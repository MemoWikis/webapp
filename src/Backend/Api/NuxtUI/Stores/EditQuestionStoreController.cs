public class EditQuestionStoreController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    IHttpContextAccessor _httpContextAccessor,
    ImageStore _imageStore) : ApiBaseController
{
    public class UploadContentImageRequest
    {
        public int QuestionId { get; set; } = -1;
        public IFormFile File { get; set; }
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public string UploadContentImage([FromForm] UploadContentImageRequest form)
    {
        if (form.QuestionId > 0 && !_permissionCheck.CanEditQuestion(form.QuestionId))
            throw new Exception("No Upload rights");

        Logg.r.Information("UploadContentImage {id}, {file}", form.QuestionId, form.File);

        var url = _imageStore.RunQuestionContentUploadAndGetPath(
            form.File,
            form.QuestionId,
            _sessionUser.UserId,
            _sessionUser.User.Name);

        return url;
    }

    public record struct DeleteContentImagesRequest(int id, string[] imageUrls);
    
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public void DeleteContentImages([FromBody] DeleteContentImagesRequest req)
    {
        var imageSettings = new QuestionContentImageSettings(req.id, _httpContextAccessor);
        var deleteImage = new DeleteImage();

        var filenames = new List<string>();

        foreach (var path in req.imageUrls)
            filenames.Add(Path.GetFileName(path));

        deleteImage.Run(imageSettings.BasePath, filenames);
    }
}