using System;
using System.Net;
using System.Web.Mvc;
using TrueOrFalse;

namespace VueApp;

public class ImageUploadModalController : BaseController
{
    [HttpPost]
    public JsonResult GetWikimediaPreview(string url)
    {
        var result = Resolve<WikiImageMetaLoader>().Run(url, 200);
        return Json(new
        {
            imageFound = !result.ImageNotFound,
            imageThumbUrl = result.ImageUrl
        });
    }
}