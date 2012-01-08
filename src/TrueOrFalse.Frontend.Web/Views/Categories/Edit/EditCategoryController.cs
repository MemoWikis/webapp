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
    private const string _viewPathSubCategoryRow = "~/Views/Categories/Edit/SubCategoryRow.ascx";
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
        model.SubCategories = new List<SubCategoryRowModel>();
        model.SubCategories.Add(new SubCategoryRowModel());
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
        PopulateSubCategoriesFromPost(model);

        var category = _categoryRepository.GetById(id);
        model.UpdateCategory(category);
        _categoryRepository.Update(category);

        return View(_viewPath, model);
    }

    [HttpPost]
    public ViewResult Create(EditCategoryModel model)
    {
        PopulateSubCategoriesFromPost(model);

        _categoryRepository.Create(model.ConvertToCategory());

        model.Message = new SuccessMessage(string.Format("Die Kategorie <strong>'{0}'</strong> wurde angelegt.", model.Name));

        return View(_viewPath, model);
    }

    private void PopulateSubCategoriesFromPost(EditCategoryModel model)
    {
        model.SubCategories = new List<SubCategoryRowModel>();

        foreach (var rowData in from string postKey in Request.Form.Keys
                                where postKey.StartsWith("row[")
                                let id = postKey.Split('.').First()
                                let property = postKey.Split('.').Last()
                                let value = Request.Form[postKey]
                                let item = new KeyValuePair<string, string>(property, value)
                                group item by id
                                into groupedItems
                                select groupedItems.ToDictionary(p => p.Key, p => p.Value))

        {
            model.SubCategories.Add(
                new SubCategoryRowModel
                    {
                        Name = rowData["Name"],
                        Type = rowData["Type"],
                        Id = Convert.ToInt32(rowData["Id"])
                    });
        }
    }

    public ViewResult AddSubCategoryRow()
    {
        return View(_viewPathSubCategoryRow, new SubCategoryRowModel());
    }

}
