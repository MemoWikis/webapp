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

    public readonly record struct GetWikimediaPreviewJson(string url);
    [HttpPost]
    public JsonResult GetWikimediaPreview([FromBody] GetWikimediaPreviewJson json)
    {
        var result = WikiImageMetaLoader.Run(json.url, 200);
        return Json(new
        {
            imageFound = !result.ImageNotFound,
            imageThumbUrl = result.ImageUrl
        });
    }

    public readonly record struct SaveWikimediaImageJson(int topicId, string url);
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public bool SaveWikimediaImage([FromBody] SaveWikimediaImageJson json)
    {
        if (json.url == null || !_permissionCheck.CanEditCategory(json.topicId))
            return false;

        _imageStore.RunWikimedia<CategoryImageSettings>(json.url, json.topicId, ImageType.Category, _sessionUser.UserId);
        return true;
    }

    public class SaveCustomImageJson
    {
        public int topicId { get; set; }
        public string licenseGiverName { get; set; }
        public IFormFile file { get; set; }
    }
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public bool SaveCustomImage([FromForm] SaveCustomImageJson form)
    {
        if (form.file == null || !_permissionCheck.CanEditCategory(form.topicId))
            return false;

        _imageStore.RunUploaded<CategoryImageSettings>(form.file, form.topicId, _sessionUser.UserId, form.licenseGiverName);

        return true;
    }
}