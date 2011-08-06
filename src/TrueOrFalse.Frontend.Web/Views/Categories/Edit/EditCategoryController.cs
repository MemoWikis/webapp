using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core.Web;

public class EditCategoryController : Controller
{
    private const string _viewPathClassificationRow = "~/Views/Categories/Edit/ClassificationRow.ascx";
    private const string _viewPath = "~/Views/Categories/Edit/EditCategory.aspx";

    public EditCategoryController()
    {
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
    public ViewResult Create(EditCategoryModel editCategoryModel)
    {
        editCategoryModel.Classifications = new List<ClassificationRowModel>();

        foreach(var rowData in from string postKey in Request.Form.Keys
                                 where postKey.StartsWith("row[")
                                 let id = postKey.Split('.').First()
                                 let property = postKey.Split('.').Last()
                                 let value = Request.Form[postKey]
                                 let item = new KeyValuePair<string,string>(property, value)
                                 group item by id into groupedItems
                                 select groupedItems.ToDictionary(p => p.Key, p=> p.Value))

        {
            editCategoryModel.Classifications.Add(
                new ClassificationRowModel
                    {
                        Name = rowData["Name"],
                        Type = rowData["Type"]
                    });
        }

        return View(_viewPath, editCategoryModel);
    }

    public ViewResult AddClassificationRow()
    {
        return View(_viewPathClassificationRow, new ClassificationRowModel());
    }

}
