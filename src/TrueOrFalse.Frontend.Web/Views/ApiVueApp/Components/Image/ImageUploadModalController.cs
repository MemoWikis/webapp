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
    private readonly PermissionCheck _permissionCheck;

    public ImageUploadModalController(SessionUser sessionUser,
        ImageStore imagestore,
        WikiImageMetaLoader wikiImageMetaLoader,
        PermissionCheck permissionCheck) : base(sessionUser)
    {
        _imagestore = imagestore;
        _wikiImageMetaLoader = wikiImageMetaLoader;
        _permissionCheck = permissionCheck;
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
        if (url == null || !_permissionCheck.CanEditCategory(topicId))
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
        if (form.file == null || !_permissionCheck.CanEditCategory(form.topicId))
            return false;

        _imagestore.RunUploaded<CategoryImageSettings>(form.file, form.topicId, _sessionUser.UserId, form.licenseGiverName);

        return true;
    }
}