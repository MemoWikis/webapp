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
                ImageThumbUrl = result.ImageUrl
            }
        };
    }

    [HttpPost]
    public FineUploaderResult File(FineUpload upload)
    {
        using (var inputStream = upload.InputStream)
        {
            var tmpImage = _sessionUiData.TmpImagesStore.Add(inputStream, 200);
            return new FineUploaderResult(true, new { FilePath = tmpImage.PathPreview, Guid = tmpImage.Guid });
        }        
    }
}