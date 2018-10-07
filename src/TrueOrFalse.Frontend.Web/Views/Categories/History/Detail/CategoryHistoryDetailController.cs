using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

public class CategoryHistoryDetailController : Controller
{
    public ActionResult Detail(int categoryChangeId)
    {
        var categoryChange = Sl.CategoryChangeRepo.GetByIdEager(categoryChangeId);
        var previousChange = Sl.CategoryChangeRepo.GetForCategory(categoryChange.Category.Id)
            .OrderByDescending(cc => cc.DateCreated)
            .SkipWhile(previous => previous.DateCreated >= categoryChange.DateCreated)
            .FirstOrDefault();

        return View("~/Views/Categories/History/Detail/CategoryHistoryDetail.aspx", new CategoryHistoryDetailModel(categoryChange, previousChange));
    }
}