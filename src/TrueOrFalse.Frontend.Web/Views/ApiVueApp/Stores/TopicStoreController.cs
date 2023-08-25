using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class TopicStoreController : BaseController
{
    private readonly PermissionCheck _permissionCheck;
    private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;
    private readonly CategoryRepository _categoryRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public TopicStoreController(SessionUser sessionUser,
        PermissionCheck permissionCheck,
        KnowledgeSummaryLoader knowledgeSummaryLoader,
        CategoryRepository categoryRepository,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment) : base(sessionUser)
    {
        _permissionCheck = permissionCheck;
        _knowledgeSummaryLoader = knowledgeSummaryLoader;
        _categoryRepository = categoryRepository;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }
    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult SaveTopic(int id, string name, bool saveName, string content, bool saveContent)
    {
        if (!_permissionCheck.CanEditCategory(id))
            return Json("Dir fehlen leider die Rechte um die Seite zu bearbeiten");

        var categoryCacheItem = EntityCache.GetCategory(id);
        var category = _categoryRepository.GetById(categoryCacheItem.Id);

        if (categoryCacheItem == null || category == null)
            return Json(false);

        if (saveName)
        {
            categoryCacheItem.Name = name;
            category.Name = name;
        }

        if (saveContent)
        {
            categoryCacheItem.Content = content;
            category.Content = content;
        }
        EntityCache.AddOrUpdate(categoryCacheItem);
        _categoryRepository.Update(category, _sessionUser.User, type: CategoryChangeType.Text);

        return Json(true);
    }

    [HttpGet]
    public string GetTopicImageUrl(int id)
    {
        if (_permissionCheck.CanViewCategory(id))
            return new CategoryImageSettings(id, _httpContextAccessor, _webHostEnvironment).GetUrl_128px(asSquare: true).Url;

        return "";
    }


    [HttpGet]
    public JsonResult GetUpdatedKnowledgeSummary(int id)
    {
        var knowledgeSummary = _knowledgeSummaryLoader.RunFromMemoryCache(id, _sessionUser.UserId);

        return Json(new
        {
            notLearned = knowledgeSummary.NotLearned + knowledgeSummary.NotInWishknowledge,
            needsLearning = knowledgeSummary.NeedsLearning,
            needsConsolidation = knowledgeSummary.NeedsConsolidation,
            solid = knowledgeSummary.Solid,
        });
    }
}

