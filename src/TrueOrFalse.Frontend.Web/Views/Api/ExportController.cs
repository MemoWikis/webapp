using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Web.Context;
using TrueOrFalse.Frontend.Web.Code;


public class ExportController : Controller
{
    public ExportController()
    {
       
    }

    public ActionResult Questions()
    {
        return new ViewResult();
    }
}