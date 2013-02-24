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
    public FineUploaderResult UploadImage(int? id, FineUpload upload)
    {
        if (id == null)
        {
            var tmpImage = new TmpImageStore().Add(upload.InputStream, 200);
            return new FineUploaderResult(true, new { filePath = tmpImage.PathPreview });
        }

        var dir = @"c:\upload\path";
        var filePath = Path.Combine(dir, upload.Filename);
        try { upload.SaveAs(filePath); }
        catch (Exception ex) { return new FineUploaderResult(false, error: ex.Message); }

        // the anonymous object in the result below will be convert to json and set back to the browser
        return new FineUploaderResult(true, new { filePath = 12345 });
    }
}