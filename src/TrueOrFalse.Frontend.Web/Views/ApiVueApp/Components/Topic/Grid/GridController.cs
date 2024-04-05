using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static TopicGridManager;

namespace VueApp;

public class GridController(
        PermissionCheck _permissionCheck,
        SessionUser _sessionUser,
        ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
        IHttpContextAccessor _httpContextAccessor,
        KnowledgeSummaryLoader _knowledgeSummaryLoader,
        QuestionReadingRepo _questionReadingRepo) : Controller
{
    [HttpGet]
    public GetItemJson GetItem([FromRoute] int id)
    {
        var topic = EntityCache.GetCategory(id);
        if (topic == null)
            return new GetItemJson(false, FrontendMessageKeys.Error.Default);
        if (!_permissionCheck.CanView(topic))
            return new GetItemJson(false, FrontendMessageKeys.Error.Category.MissingRights);

        var gridItem = new TopicGridManager(
                _permissionCheck,
                _sessionUser,
                _imageMetaDataReadingRepo,
                _httpContextAccessor,
                _knowledgeSummaryLoader,
                _questionReadingRepo)
            .BuildGridTopicItem(topic);

        return new GetItemJson(true, "", gridItem);
    }

    public readonly struct GetItemJson(bool Success, string MessageKey = "", GridTopicItem Data = null);
}