using System;
using System.IO;
using System.Web.Mvc;
using FineUploader;
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

    [HttpPost]
    public FineUploaderResult UploadImage(FineUpload upload)
    {
        var tmpImage = _sessionUiData.TmpImagesStore.Add(upload.InputStream, 200);
        return new FineUploaderResult(true, new { filePath = tmpImage.Path });
    }
}