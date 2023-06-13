using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;

namespace VueApp;

public class ImageUploadModalController : BaseController
{
    private readonly ImageStore _imagestore;
    private readonly WikiImageMetaLoader _wikiImageMetaLoader;

    public ImageUploadModalController(SessionUser sessionUser, ImageStore imagestore,WikiImageMetaLoader wikiImageMetaLoader) : base(sessionUser)
    {
        _imagestore = imagestore;
        _wikiImageMetaLoader = wikiImageMetaLoader;
    }

    [HttpPost]
    public JsonResult GetWikimediaPreview(string url)
    {
        var result = _wikiImageMetaLoader.Run(url, 200);
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

        _imagestore.RunWikimedia<CategoryImageSettings>(url, topicId, ImageType.Category, _sessionUser.UserId);
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

        _imagestore.RunUploaded<CategoryImageSettings>(form.file, form.topicId, _sessionUser.UserId, form.licenseGiverName);

        return true;
    }
}