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

        return View("~/Views/Categories/History/Detail/CategoryHistoryDetail.aspx", new CategoryHistoryDetailModel(categoryChange));
    }
}