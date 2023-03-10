using System.Linq;
using System.Web.Mvc;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;
using TrueOrFalse.Web;

namespace VueApp;

public class HistoryAllTopicsOverviewController : BaseController
{

    [HttpGet]
    public JsonResult Get()
    {
        return Json(new {}, JsonRequestBehavior.AllowGet);
    }

}