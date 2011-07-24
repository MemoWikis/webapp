using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class EditCategoryController : Controller
{
    private const string _viewLocation = "~/Views/Categories/Edit/EditCategory.aspx";

    public ActionResult Create()
    {
        var editCategoryModel = new EditCategoryModel();
        editCategoryModel.Classifications.Add(new ClassificationRowModel());
        return View(_viewLocation, editCategoryModel);
    }


}
