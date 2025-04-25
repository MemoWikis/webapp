using static PageGridManager;

public class GridController(
    PermissionCheck _permissionCheck,
    SessionUser _sessionUser,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    IHttpContextAccessor _httpContextAccessor,
    KnowledgeSummaryLoader _knowledgeSummaryLoader,
    QuestionReadingRepo _questionReadingRepo) : ApiBaseController
{
    [HttpGet]
    public GetItemJson GetItem([FromRoute] int id)
    {
        var page = EntityCache.GetPage(id);
        if (page == null)
            return new GetItemJson(false, FrontendMessageKeys.Error.Default);
        if (!_permissionCheck.CanView(page))
            return new GetItemJson(false, FrontendMessageKeys.Error.Page.MissingRights);

        var gridItem = new PageGridManager(
                _permissionCheck,
                _sessionUser,
                _imageMetaDataReadingRepo,
                _httpContextAccessor,
                _knowledgeSummaryLoader,
                _questionReadingRepo)
            .BuildGridPageItem(page);

        return new GetItemJson(true, "", gridItem);
    }

    public readonly record struct GetItemJson(
        bool Success,
        string MessageKey = "",
        GridPageItem? Data = null);
}