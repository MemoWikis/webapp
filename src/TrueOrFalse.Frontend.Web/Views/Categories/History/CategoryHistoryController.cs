using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class CategoryHistoryController : Controller
{
    private const string _viewLocation = "~/Views/Categories/History/CategoryHistory.aspx";

    public ActionResult List(int categoryId)
    {
        var category = Sl.CategoryRepo.GetById(categoryId);
        
        var categoryChanges = Sl.CategoryChangeRepo.GetForCategory(categoryId);

        return View(_viewLocation, new CategoryHistoryModel(category, categoryChanges, categoryId));
    }
}
