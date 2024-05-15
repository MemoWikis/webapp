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
    public readonly record struct GetWikimediaPreviewJson(string Url);

    public readonly record struct GetWikimediaPreviewResult(bool ImageFound, string ImageThumbUrl);

    [HttpPost]
    public GetWikimediaPreviewResult GetWikimediaPreview([FromBody] GetWikimediaPreviewJson json)
    {
        var result = WikiImageMetaLoader.Run(json.Url, 200);
        return new
        (
            ImageFound: !result.ImageNotFound,
            ImageThumbUrl: result.ImageUrl
        );
    }

    public readonly record struct SaveWikimediaImageJson(int TopicId, string Url);
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public bool SaveWikimediaImage([FromBody] SaveWikimediaImageJson json)
    {
        if (json.Url == null || !permissionCheck.CanEditCategory(json.TopicId))
            return false;

        imageStore.RunWikimedia<CategoryImageSettings>(
            json.Url,
            json.TopicId,
            ImageType.Category,
            _sessionUser.UserId);
        return true;
    }

    public readonly record struct SaveCustomImageForm(int TopicId, string LicenseGiverName, IFormFile File);
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public bool SaveCustomImage([FromForm] SaveCustomImageForm form)
    {
        if (form.File == null || !permissionCheck.CanEditCategory(form.TopicId))
            return false;

        imageStore.RunUploaded<CategoryImageSettings>(
            form.File,
            form.TopicId,
            _sessionUser.UserId,
            form.LicenseGiverName);

        return true;
    }
}