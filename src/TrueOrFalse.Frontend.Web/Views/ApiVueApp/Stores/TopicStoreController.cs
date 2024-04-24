using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class TopicStoreController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    KnowledgeSummaryLoader _knowledgeSummaryLoader,
    CategoryRepository _categoryRepository,
    IHttpContextAccessor _httpContextAccessor,
    TopicGridManager _gridItemLogic) : BaseController(_sessionUser)
{
    public readonly record struct SaveTopicParam(
        int id,
        string name,
        bool saveName,
        string content,
        bool saveContent);

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult SaveTopic([FromBody] SaveTopicParam param)
    {
        if (!_permissionCheck.CanEditCategory(param.id))
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Category.MissingRights
            });

        var categoryCacheItem = EntityCache.GetCategory(param.id);
        var category = _categoryRepository.GetById(param.id);

        if (categoryCacheItem == null || category == null)
            return Json(false);

        if (param.saveName)
        {
            categoryCacheItem.Name = param.name;
            category.Name = param.name;
        }

        if (param.saveContent)
        {
            categoryCacheItem.Content = param.content;
            category.Content = param.content;
        }

        EntityCache.AddOrUpdate(categoryCacheItem);
        _categoryRepository.Update(category, _sessionUser.UserId, type: CategoryChangeType.Text);

        return Json(new RequestResult
        {
            success = true
        });
    }

    [HttpGet]
    public string GetTopicImageUrl([FromRoute] int id)
    {
        if (_permissionCheck.CanViewCategory(id))
            return new CategoryImageSettings(id, _httpContextAccessor).GetUrl_128px(asSquare: true)
                .Url;

        return "";
    }

    [HttpGet]
    public JsonResult GetUpdatedKnowledgeSummary([FromRoute] int id)
    {
        var sessionuserId = _sessionUser == null ? -1 : _sessionUser.UserId;
        var knowledgeSummary = _knowledgeSummaryLoader.RunFromMemoryCache(id, sessionuserId);

        return Json(new
        {
            notLearned = knowledgeSummary.NotLearned + knowledgeSummary.NotInWishknowledge,
            needsLearning = knowledgeSummary.NeedsLearning,
            needsConsolidation = knowledgeSummary.NeedsConsolidation,
            solid = knowledgeSummary.Solid,
        });
    }

    [HttpGet]
    public TopicGridManager.GridTopicItem[] GetGridTopicItems([FromRoute] int id)
    {
        return _gridItemLogic.GetChildren(id);
    }
}