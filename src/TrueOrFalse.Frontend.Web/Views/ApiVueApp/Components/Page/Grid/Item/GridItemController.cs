using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class GridItemController(
    PermissionCheck _permissionCheck,
    SessionUser _sessionUser,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    IHttpContextAccessor _httpContextAccessor,
    KnowledgeSummaryLoader _knowledgeSummaryLoader,
    QuestionReadingRepo _questionReadingRepo) : Controller
{
    public readonly record struct GetChildrenResult(
        bool Success,
        string MessageKey = "",
        PageGridManager.GridPageItem[] Data = null);

    [HttpGet]
    public GetChildrenResult GetChildren([FromRoute] int id)
    {
        var page = EntityCache.GetPage(id);
        if (!_permissionCheck.CanView(page))
            return new GetChildrenResult(
                Success: false,
                MessageKey: FrontendMessageKeys.Error.Page.MissingRights);

        var children = new PageGridManager(
            _permissionCheck,
            _sessionUser,
            _imageMetaDataReadingRepo,
            _httpContextAccessor,
            _knowledgeSummaryLoader,
            _questionReadingRepo).GetChildren(id);

        return new GetChildrenResult(Success: true, MessageKey: "", Data: children);
    }

    public readonly record struct GetItemResult(
        bool Success,
        string MessageKey,
        PageGridManager.GridPageItem Data);

    [HttpGet]
    public GetItemResult GetItem([FromRoute] int id)
    {
        var page = EntityCache.GetPage(id);
        if (!_permissionCheck.CanView(page))
            return new GetItemResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.MissingRights
            };

        var gridItem = new PageGridManager(
            _permissionCheck,
            _sessionUser,
            _imageMetaDataReadingRepo,
            _httpContextAccessor,
            _knowledgeSummaryLoader,
            _questionReadingRepo).BuildGridPageItem(page);
        return new GetItemResult { Success = true, Data = gridItem };
    }
}