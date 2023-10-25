using HelperClassesControllers;
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
    public JsonResult GetWikimediaPreview([FromBody] ImageUploadModalHelper.GetWikimediaPreviewJson json)
    {
        var result = WikiImageMetaLoader.Run(json.url, 200);
        return Json(new
        {
            imageFound = !result.ImageNotFound,
            imageThumbUrl = result.ImageUrl
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public bool SaveWikimediaImage([FromBody] ImageUploadModalHelper.SaveWikimediaImageJson json)
    {
        if (json.url == null || !_permissionCheck.CanEditCategory(json.topicId))
            return false;

        _imageStore.RunWikimedia<CategoryImageSettings>(json.url, json.topicId, ImageType.Category, _sessionUser.UserId);
        return true;
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public bool SaveCustomImage([FromForm] ImageUploadModalHelper.CustomImageFormdata form)
    {
        if (form.file == null || !_permissionCheck.CanEditCategory(form.topicId))
            return false;

        _imageStore.RunUploaded<CategoryImageSettings>(form.file, form.topicId, _sessionUser.UserId, form.licenseGiverName);

        return true;
    }
}