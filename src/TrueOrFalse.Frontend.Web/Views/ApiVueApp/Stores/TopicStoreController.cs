using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class TopicStoreController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    KnowledgeSummaryLoader _knowledgeSummaryLoader,
    CategoryRepository _categoryRepository,
    IHttpContextAccessor _httpContextAccessor,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    QuestionReadingRepo _questionReadingRepo) : Controller
{
    public readonly record struct SaveTopicParam(
        int id,
        string name,
        bool saveName,
        string content,
        bool saveContent);

    public readonly record struct TopicResult(bool Success, string MessageKey);

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public TopicResult SaveTopic([FromBody] SaveTopicParam param)
    {
        if (!_permissionCheck.CanEditCategory(param.id))
            return new TopicResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Category.MissingRights
            };

        var categoryCacheItem = EntityCache.GetCategory(param.id);
        var category = _categoryRepository.GetById(param.id);
        //todo(Jun) Please adjust, this return was Json(false). 
        if (categoryCacheItem == null || category == null)
            return new TopicResult { Success = false };

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

        return new TopicResult
        {
            Success = true
        };
    }

    [HttpGet]
    public string GetTopicImageUrl([FromRoute] int id)
    {
        if (_permissionCheck.CanViewCategory(id))
            return new CategoryImageSettings(id, _httpContextAccessor).GetUrl_128px(asSquare: true)
                .Url;

        return "";
    }

    public readonly record struct KnowledgeSummaryResult(
        int NotLearned,
        int NeedsLearning,
        int NeedsConsolidation,
        int Solid);

    [HttpGet]
    public KnowledgeSummaryResult GetUpdatedKnowledgeSummary([FromRoute] int id)
    {
        var sessionuserId = _sessionUser == null ? -1 : _sessionUser.UserId;
        var knowledgeSummary = _knowledgeSummaryLoader.RunFromMemoryCache(id, sessionuserId);

        return new KnowledgeSummaryResult
        {
            NotLearned = knowledgeSummary.NotLearned + knowledgeSummary.NotInWishknowledge,
            NeedsLearning = knowledgeSummary.NeedsLearning,
            NeedsConsolidation = knowledgeSummary.NeedsConsolidation,
            Solid = knowledgeSummary.Solid,
        };
    }

    [HttpGet]
    public TopicGridManager.GridTopicItem[] GetGridTopicItems([FromRoute] int id)
    {
        return new TopicGridManager(
            _permissionCheck,
            _sessionUser,
            _imageMetaDataReadingRepo,
            _httpContextAccessor,
            _knowledgeSummaryLoader,
            _questionReadingRepo).GetChildren(id);
    }
}