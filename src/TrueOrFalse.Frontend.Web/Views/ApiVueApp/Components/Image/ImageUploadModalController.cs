using System;
using System.Net;
using System.Web;
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

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public bool SaveWikimediaImage(int topicId, string url)
    {
        if (url == null || !PermissionCheck.CanEditCategory(topicId))
            return false;

        Resolve<ImageStore>().RunWikimedia<CategoryImageSettings>(url, topicId, ImageType.Category, SessionUserLegacy.UserId);
        return true;
    }

    public class CustomImageFormdata
    {
        public int topicId { get; set; }
        public string licenseGiverName { get; set; }
        public HttpPostedFileBase file { get; set; }
    }

    [AccessOnlyAsLoggedIn]

    [HttpPost]
    public bool SaveCustomImage(CustomImageFormdata form)
    {
        if (form.file == null || !PermissionCheck.CanEditCategory(form.topicId))
            return false;

        Resolve<ImageStore>().RunUploaded<CategoryImageSettings>(form.file, form.topicId, SessionUserLegacy.UserId, form.licenseGiverName);

        return true;
    }
}