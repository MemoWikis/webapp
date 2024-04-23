using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrueOrFalse;

namespace VueApp;

public class ImageUploadModalController(
    SessionUser sessionUser,
    PermissionCheck permissionCheck,
    ImageStore imageStore)
    : BaseController(sessionUser)
{
    public readonly record struct GetWikimediaPreviewJson(string url);

    public readonly record struct GetWikimediaPreviewResult(bool ImageFound, string ImageThumbUrl);

    [HttpPost]
    public GetWikimediaPreviewResult GetWikimediaPreview([FromBody] GetWikimediaPreviewJson json)
    {
        var result = WikiImageMetaLoader.Run(json.url, 200);
        return new
        (
            ImageFound: !result.ImageNotFound,
            ImageThumbUrl: result.ImageUrl
        );
    }

    public readonly record struct SaveWikimediaImageJson(int topicId, string url);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public bool SaveWikimediaImage([FromBody] SaveWikimediaImageJson json)
    {
        if (json.url == null || !permissionCheck.CanEditCategory(json.topicId))
            return false;

        imageStore.RunWikimedia<CategoryImageSettings>(
            json.url,
            json.topicId,
            ImageType.Category,
            _sessionUser.UserId);
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
        if (form.file == null || !permissionCheck.CanEditCategory(form.topicId))
            return false;

        imageStore.RunUploaded<CategoryImageSettings>(
            form.file,
            +form.topicId,
            _sessionUser.UserId,
            form.licenseGiverName);

        return true;
    }
}