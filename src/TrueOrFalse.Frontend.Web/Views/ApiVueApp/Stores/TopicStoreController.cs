using HelperClassesControllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class TopicStoreController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly PermissionCheck _permissionCheck;
    private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;
    private readonly CategoryRepository _categoryRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TopicStoreController(SessionUser sessionUser,
        PermissionCheck permissionCheck,
        KnowledgeSummaryLoader knowledgeSummaryLoader,
        CategoryRepository categoryRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _sessionUser = sessionUser;
        _permissionCheck = permissionCheck;
        _knowledgeSummaryLoader = knowledgeSummaryLoader;
        _categoryRepository = categoryRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult SaveTopic([FromBody] TopicStoreHelper.SaveTopicJson json)
    {
        if (!_permissionCheck.CanEditCategory(json.id))
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Category.MissingRights
            });

        var categoryCacheItem = EntityCache.GetCategory(json.id);
        var category = _categoryRepository.GetById(json.id);

        if (categoryCacheItem == null || category == null)
            return Json(false);

        if (json.saveName)
        {
            categoryCacheItem.Name = json.name;
            category.Name = json.name;
        }

        if (json.saveContent)
        {
            categoryCacheItem.Content = json.content;
            category.Content = json.content;
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
            return new CategoryImageSettings(id, _httpContextAccessor).GetUrl_128px(asSquare: true).Url;

        return "";
    }


    [HttpGet]
    public JsonResult GetUpdatedKnowledgeSummary([FromRoute] int id)
    {
        var sessionuserId = _sessionUser == null ? -1 : _sessionUser.UserId;   
        var knowledgeSummary = _knowledgeSummaryLoader.RunFromMemoryCache(id,  sessionuserId);

        return Json(new
        {
            notLearned = knowledgeSummary.NotLearned + knowledgeSummary.NotInWishknowledge,
            needsLearning = knowledgeSummary.NeedsLearning,
            needsConsolidation = knowledgeSummary.NeedsConsolidation,
            solid = knowledgeSummary.Solid,
        });
    }
}

