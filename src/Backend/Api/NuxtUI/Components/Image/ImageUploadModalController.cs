using TrueOrFalse;

public class ImageUploadModalController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    ImageStore _imageStore) : ApiBaseController
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

    public readonly record struct SaveWikimediaImageJson(int PageId, string Url);
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public bool SaveWikimediaImage([FromBody] SaveWikimediaImageJson json)
    {
        if (json.Url == null || !_permissionCheck.CanEditPage(json.PageId))
            return false;

        _imageStore.RunWikimedia<PageImageSettings>(
            json.Url,
            json.PageId,
            ImageType.Page,
            _sessionUser.UserId);
        return true;
    }

    public class SaveCustomImageForm
    {
        public int PageId { get; set; }
        public string LicenseGiverName { get; set; }
        public IFormFile File { get; set; }
    }
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public bool SaveCustomImage([FromForm] SaveCustomImageForm form)
    {
        if (form.File == null || !_permissionCheck.CanEditPage(form.PageId))
            return false;

        _imageStore.RunUploaded<PageImageSettings>(
            form.File,
            form.PageId,
            _sessionUser.UserId,
            form.LicenseGiverName);

        return true;
    }
}