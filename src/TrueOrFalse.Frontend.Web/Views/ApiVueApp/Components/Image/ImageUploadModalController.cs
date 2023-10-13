using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrueOrFalse;

namespace VueApp;

public class ImageUploadModalController
    : BaseController
{
    private readonly PermissionCheck _permissionCheck;
    private readonly ImageStore _imageStore;

    public ImageUploadModalController(SessionUser sessionUser,
        PermissionCheck permissionCheck,
        ImageStore imageStore) : base(sessionUser)
    {
        _permissionCheck = permissionCheck;
        _imageStore = imageStore;
    }


    [HttpPost]
    public JsonResult GetWikimediaPreview(string url)
    {
        var result = WikiImageMetaLoader.Run(url, 200);
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

        _imageStore.RunWikimedia<CategoryImageSettings>(url, topicId, ImageType.Category, _sessionUser.UserId);
        return true;
    }

    public class CustomImageFormdata
    {
        public int topicId { get; set; }
        public string licenseGiverName { get; set; }
        public IFormFile file { get; set; }
    }

    //[AccessOnlyAsLoggedIn]

    //[HttpPost]
    public bool SaveCustomImage([FromForm] CustomImageFormdata form)
    {
        if (form.file == null || !_permissionCheck.CanEditCategory(form.topicId))
            return false;

        _imageStore.RunUploaded<CategoryImageSettings>(form.file, form.topicId, _sessionUser.UserId, form.licenseGiverName);

        return true;
    }
}