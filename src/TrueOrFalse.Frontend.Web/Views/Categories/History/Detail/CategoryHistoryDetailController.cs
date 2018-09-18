using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

public class CategoryHistoryDetailController
{
    public ActionResult Detail(int detailId)
    {
        return View("~/Views/Categories/History/Detail/CategoryHistoryDetail.aspx", new CategoryHistoryDetailController());
    }
}