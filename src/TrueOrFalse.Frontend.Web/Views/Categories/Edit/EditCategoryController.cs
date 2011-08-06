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
    private const string _viewPathClassificationRow = "~/Views/Categories/Edit/ClassificationRow.ascx";
    private const string _viewPath = "~/Views/Categories/Edit/EditCategory.aspx";

    public EditCategoryController(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        ActionInvoker = new JavaScriptActionInvoker();
    }

    public ViewResult Create()
    {
        var model = new EditCategoryModel();
        model.Name = "Some name";
        model.Classifications = new List<ClassificationRowModel>();
        model.Classifications.Add(new ClassificationRowModel());
        return View(_viewPath, model);
    }

    [HttpPost]
    public ViewResult Create(EditCategoryModel model)
    {
        model.Classifications = new List<ClassificationRowModel>();

        foreach(var rowData in from string postKey in Request.Form.Keys
                                 where postKey.StartsWith("row[")
                                 let id = postKey.Split('.').First()
                                 let property = postKey.Split('.').Last()
                                 let value = Request.Form[postKey]
                                 let item = new KeyValuePair<string,string>(property, value)
                                 group item by id into groupedItems
                                 select groupedItems.ToDictionary(p => p.Key, p=> p.Value))

        {
            model.Classifications.Add(
                new ClassificationRowModel
                    {
                        Name = rowData["Name"],
                        Type = rowData["Type"]
                    });
        }

        _categoryRepository.Create(model.ConvertToCategory());

        return View(_viewPath, model);
    }

    public ViewResult AddClassificationRow()
    {
        return View(_viewPathClassificationRow, new ClassificationRowModel());
    }

}
