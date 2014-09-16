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

    public ViewResult Create(string name, string parent, string type)
    {
        var model = new EditCategoryModel {Name = name ?? "", PreselectedType = !String.IsNullOrEmpty(type) ? (CategoryType)Enum.Parse(typeof(CategoryType), type) : CategoryType.Standard };

        if (!String.IsNullOrEmpty(parent))
            model.ParentCategories.Add(_categoryRepository.GetById(Convert.ToInt32(parent)));

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

        var categoryAllowed = new CategoryNameAllowed();

        model.FillReleatedCategoriesFromPostData(Request.Form);
        model.UpdateCategory(category);
        if (model.Name != category.Name && categoryAllowed.No(model, category.Type)){
            model.Message = new ErrorMessage(string.Format("Es existiert bereits eine Kategorie mit dem Namen <strong>'{0}'</strong>.",
                                                            categoryAllowed.ExistingCategories.First().Name));
        } else {
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
        model.FillReleatedCategoriesFromPostData(Request.Form);
        
        var convertResult = model.ConvertToCategory();
        if (convertResult.HasError){

            if(convertResult.TypeModel == null)
                throw new Exception("Dear developer, please assign the type model!");

            EditCategoryTypeModel.SaveToSession(convertResult.TypeModel, convertResult.Category);
            model.Message = convertResult.ErrorMessage;
            return View(_viewPath, model);
        }

        var category = convertResult.Category;
        category.Creator = _sessionUser.User;

        var categoryNameAllowed = new CategoryNameAllowed();
        if (categoryNameAllowed.No(category))
        {
            model.Message = new ErrorMessage(
                string.Format("Die Kategorie <strong>'{0}'</strong> existiert bereits. " +
                              "Klicke <a href=\"{1}\">hier</a>, um sie zu bearbeiten.",
                              categoryNameAllowed.ExistingCategories.First().Name,
                              Url.Action("Edit", new { id = categoryNameAllowed.ExistingCategories.First().Id })));

            return View(_viewPath, model);
        }


        _categoryRepository.Create(category);
        StoreImage(category.Id);

        EditCategoryTypeModel.RemoveRecentTypeModelFromSession();

        TempData["createCategoryMsg"] 
            = new SuccessMessage(string.Format(
                 "Die Kategorie <strong>'{0}'</strong> wurde angelegt.<br>" + 
                 "Du kannst die Kategorie jetzt bearbeiten" +
                 " oder eine <a href='/Kategorien/Erstelle'>neue Kategorie anlegen</a>.", 
                category.Name));

        return Redirect("/Kategorien/Bearbeite/" + category.Id);
    }

    public ActionResult DetailsPartial(int? categoryId, CategoryType type, string typeModelGuid)
    {
        Category category = null;

        if (categoryId.HasValue && categoryId.Value > 0){
            category = _categoryRepository.GetById(categoryId.Value);
        }

        return View(string.Format(_viewPathTypeControls, type), new EditCategoryTypeModel(category, type));
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