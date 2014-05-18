using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Web;
using TrueOrFalse;

public class EditCategoryController : BaseController
{
    private readonly CategoryRepository _categoryRepository;
    private const string _viewPath = "~/Views/Categories/Edit/EditCategory.aspx";
    private const string _viewPathTypeControls = "~/Views/Categories/Edit/TypeControls/{0}.ascx";

    public EditCategoryController(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        ActionInvoker = new JavaScriptActionInvoker();
    }

    public ViewResult Create(string name, string parent)
    {
        var model = new EditCategoryModel {Name = name ?? ""};

        if (!String.IsNullOrEmpty(parent))
            model.ParentCategories.Add(_categoryRepository.GetByName(parent).Name);

        return View(_viewPath, model);
    }


    public ViewResult Edit(int id)
    {
        var category = _categoryRepository.GetById(id);
        _sessionUiData.VisitedCategories.Add(new CategoryHistoryItem(category, HistoryItemType.Edit));
        
        var model = new EditCategoryModel(category){IsEditing = true};

        if (TempData["createCategoryMsg"] != null)
            model.Message = (SuccessMessage)TempData["createCategoryMsg"];

        return View(_viewPath, model);
    }

    [HttpPost]
    public ViewResult Edit(int id, EditCategoryModel model, HttpPostedFileBase file)
    {
        var category = _categoryRepository.GetById(id);
        _sessionUiData.VisitedCategories.Add(new CategoryHistoryItem(category, HistoryItemType.Edit));

        var categoryExists = new EditCategoryModel_Category_Exists();
        if (model.Name != category.Name && categoryExists.Yes(model))
        {
            model.Message = new ErrorMessage(string.Format("Es existiert bereits eine Kategorie mit dem Namen <strong>'{0}'</strong>.",
                                                           categoryExists.ExistingCategory.Name));
        }
        else
        {
            model.FillReleatedCategoriesFromPostData(Request.Form);
            model.UpdateCategory(category);
            _categoryRepository.Update(category);

            model.Message = new SuccessMessage("Die Kategorie wurde gespeichert.");
        }
        StoreImage(id);
        
        model.Init(category);
        model.IsEditing = true;

        return View(_viewPath, model);
    }

    [HttpPost]
    public ActionResult Create(EditCategoryModel model, HttpPostedFileBase file)
    {
        var categoryExists = new EditCategoryModel_Category_Exists();
        if (categoryExists.Yes(model))
        {
            model.Message = new ErrorMessage(string.Format("Die Kategorie <strong>'{0}'</strong> existiert bereits. " +
                                                           "Klicke <a href=\"{1}\">hier</a>, um sie zu bearbeiten.", 
                                                           categoryExists.ExistingCategory.Name,
                                                           Url.Action("Edit", new { id = categoryExists.ExistingCategory.Id })));

            return View(_viewPath, model);
        }

        model.FillReleatedCategoriesFromPostData(Request.Form);
        var category = model.ConvertToCategory();
        category.Creator = _sessionUser.User;
        _categoryRepository.Create(category);
        StoreImage(category.Id);

        TempData["createCategoryMsg"] 
            = new SuccessMessage(string.Format(
                 "Die Kategorie <strong>'{0}'</strong> wurde angelegt.<br>" + 
                 "Du kannst die Kategorie jetzt bearbeiten<br>" +
                 "oder eine <a href='/Kategorien/Erstelle'>neue Kategorie anlegen</a>.", 
                model.Name));

        return Redirect("/Kategorien/Bearbeite/" + category.Id);
    }

    public ActionResult DetailsPartial(int? categoryId, CategoryType type)
    {
        object model = null;

        if (categoryId.HasValue && categoryId.Value > 0)
        {
            var question = _categoryRepository.GetById(categoryId.Value);
            model = new GetQuestionSolution().Run(question);
        }

        return View(string.Format(_viewPathTypeControls, type), model);
    }

    private void StoreImage(int categoryId)
    {
        if (Request["ImageIsNew"] == "true")
        {
            if (Request["ImageSource"] == "wikimedia")
            {
                Resolve<ImageStore>().RunWikimedia<CategoryImageSettings>(
                    Request["ImageWikiFileName"], categoryId, ImageType.Category, _sessionUser.User.Id);
            }
            if (Request["ImageSource"] == "upload")
            {
                Resolve<ImageStore>().RunUploaded<CategoryImageSettings>(
                    _sessionUiData.TmpImagesStore.ByGuid(Request["ImageGuid"]), categoryId, _sessionUser.User.Id, Request["ImageLicenceOwner"]);
            }
        }
    }
}