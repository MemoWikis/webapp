using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class CategoryHistoryController : Controller
{
    private const string _viewLocation = "~/Views/Categories/History/CategoryHistory.aspx";

    public ActionResult Detail(int categoryId)
    {
        var category = Sl.CategoryRepo.GetById(categoryId);

        return View(_viewLocation, new CategoryHistoryModel(category));
    }
}
