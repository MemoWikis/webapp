using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class EditCategoryController : Controller
{
    private const string _viewLocation = "~/Views/Categories/Edit/EditCategory.aspx";

    public ViewResult Create()
    {
        var model = new EditCategoryModel();
        model.Classifications = new List<ClassificationRowModel>();
        model.Classifications.Add(new ClassificationRowModel());
        return View(_viewLocation, model);
    }

    [HttpGet]
    public ViewResult AddClassification(EditCategoryModel model)
    {
        model.Classifications.Add(new ClassificationRowModel());
        return View(_viewLocation, model);
    }

}
