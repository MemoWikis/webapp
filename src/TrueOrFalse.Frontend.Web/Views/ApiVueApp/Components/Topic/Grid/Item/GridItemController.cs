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
    public readonly record struct GetChildrenJson(bool Success, string MessageKey = "", TopicGridManager.GridTopicItem[] Data = null); 
    [HttpGet]
    public GetChildrenJson GetChildren([FromRoute] int id)
    {
        var topic = EntityCache.GetCategory(id);
        if (!_permissionCheck.CanView(topic))
            return new GetChildrenJson(
                 Success: false,MessageKey:  FrontendMessageKeys.Error.Category.MissingRights );
        var children = new TopicGridManager(
            _permissionCheck,
            _sessionUser,
            _imageMetaDataReadingRepo,
            _httpContextAccessor,
            _knowledgeSummaryLoader,
            _questionReadingRepo).GetChildren(id);
        return new GetChildrenJson( Success: true, MessageKey: "", Data: children );
    }

    [HttpGet]
    public JsonResult GetItem([FromRoute] int id)
    {
        var topic = EntityCache.GetCategory(id);
        if (!_permissionCheck.CanView(topic))
            return Json(new RequestResult
                { Success = false, MessageKey = FrontendMessageKeys.Error.Category.MissingRights });


        var gridItem = new TopicGridManager(
            _permissionCheck,
            _sessionUser,
            _imageMetaDataReadingRepo,
            _httpContextAccessor,
            _knowledgeSummaryLoader,
            _questionReadingRepo).BuildGridTopicItem(topic);
        return Json(new RequestResult { Success = true, Data = gridItem });
    }
}