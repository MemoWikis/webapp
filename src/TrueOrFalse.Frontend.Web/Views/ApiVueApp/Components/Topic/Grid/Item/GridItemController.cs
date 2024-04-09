using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class GridItemController(
    PermissionCheck _permissionCheck,
    SessionUser _sessionUser,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    IHttpContextAccessor _httpContextAccessor,
    KnowledgeSummaryLoader _knowledgeSummaryLoader,
    QuestionReadingRepo _questionReadingRepo) : BaseController(_sessionUser)
{
    public readonly record struct GridItemResult(
        bool Success,
        string MessageKey = "",
        TopicGridManager.GridTopicItem[] Data = null);

    [HttpGet]
    public GridItemResult GetChildren([FromRoute] int id)
    {
        var topic = EntityCache.GetCategory(id);
        if (!_permissionCheck.CanView(topic))
            return new GridItemResult(
                Success: false, MessageKey: FrontendMessageKeys.Error.Category.MissingRights);
        var children = new TopicGridManager(
            _permissionCheck,
            _sessionUser,
            _imageMetaDataReadingRepo,
            _httpContextAccessor,
            _knowledgeSummaryLoader,
            _questionReadingRepo).GetChildren(id);
        return new GridItemResult(Success: true, MessageKey: "", Data: children);
    }

    public readonly record struct ItemJson(
        bool Success,
        string MessageKey,
        TopicGridManager.GridTopicItem Data);

    [HttpGet]
    public ItemJson GetItem([FromRoute] int id)
    {
        var topic = EntityCache.GetCategory(id);
        if (!_permissionCheck.CanView(topic))
            return new ItemJson
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Category.MissingRights
            };

        var gridItem = new TopicGridManager(
            _permissionCheck,
            _sessionUser,
            _imageMetaDataReadingRepo,
            _httpContextAccessor,
            _knowledgeSummaryLoader,
            _questionReadingRepo).BuildGridTopicItem(topic);
        return new ItemJson { Success = true, Data = gridItem };
    }
}