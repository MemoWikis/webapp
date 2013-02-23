using System.Web.Mvc;
using TrueOrFalse;

public class ImageUploadController : BaseController
{
    [HttpPost]
    public JsonResult FromWikimedia()
    {
        var result = Resolve<WikiImageMetaLoader>().Run(Request["url"], 200);
        return new JsonResult{
            Data = new{
                ImageNotFound = result.ImageNotFound,
                ImageThumbUrl = result.ImageThumbUrl
            }
        };
    }
}