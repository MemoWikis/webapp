using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class EditCategoryController : Controller
{
    private const string _viewPathClassificationRow = "~/Views/Categories/Edit/ClassificationRow.ascx";
    private const string _viewPath = "~/Views/Categories/Edit/EditCategory.aspx";

    public ViewResult Create()
    {
        var model = new EditCategoryModel();
        model.Classifications = new List<ClassificationRowModel>();
        model.Classifications.Add(new ClassificationRowModel());
        return View(_viewPath, model);
    }

    public ViewResult AddClassificationRow()
    {
        return View(_viewPathClassificationRow, new ClassificationRowModel());
    }

}
