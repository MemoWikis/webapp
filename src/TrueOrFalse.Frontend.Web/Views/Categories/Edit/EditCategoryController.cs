using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core.Web;
using TrueOrFalse.Core;

public class EditCategoryController : Controller
{
    private readonly CategoryRepository _categoryRepository;
    private const string _viewPath = "~/Views/Categories/Edit/EditCategory.aspx";

    public EditCategoryController(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        ActionInvoker = new JavaScriptActionInvoker();
    }

    public ViewResult Create(string name)
    {
        var model = new EditCategoryModel();
        model.Name= name ?? "Some name";
        return View(_viewPath, model);
    }


    public ViewResult Edit(int id)
    {
        var model = new EditCategoryModel(_categoryRepository.GetById(id))
                        {
                            IsEditing = true
                        };
        return View(_viewPath, model);
    }

    [HttpPost]
    public ViewResult Edit(int id, EditCategoryModel model)
    {
        var category = _categoryRepository.GetById(id);

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
        }
        return View(_viewPath, model);
    }

    [HttpPost]
    public ViewResult Create(EditCategoryModel model)
    {
        var categoryExists = new EditCategoryModel_Category_Exists();
        if (categoryExists.Yes(model))
        {
            model.Message = new ErrorMessage(string.Format("Die Kategorie <strong>'{0}'</strong> existiert bereits. " +
                                                           "Klicke <a href=\"{1}\">hier</a>, um sie zu bearbeiten.", 
                                                           categoryExists.ExistingCategory.Name,
                                                           Url.Action("Edit", new { id = categoryExists.ExistingCategory.Id })));
        }
        else
        {
            model.FillReleatedCategoriesFromPostData(Request.Form);
            _categoryRepository.Create(model.ConvertToCategory());

            model.Message = new SuccessMessage(string.Format("Die Kategorie <strong>'{0}'</strong> wurde angelegt.", model.Name));
        }
        return View(_viewPath, model);
    }

}
