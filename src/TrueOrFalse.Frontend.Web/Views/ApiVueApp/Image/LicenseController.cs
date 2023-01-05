using System.Web.Mvc;

namespace VueApp;

public class ImageLicenseController : BaseController
{
    [HttpGet]
    public JsonResult GetLicenseData(int id)
    {
        return Json(new
        {

        });
    }
}