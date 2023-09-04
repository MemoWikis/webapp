using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace VueApp;

public class EditTopicRelationStoreController : BaseController
{
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGlobalSearch _search;
    private readonly PermissionCheck _permissionCheck;
    private readonly SessionUser _sessionUser;


    public EditTopicRelationStoreController(SessionUser sessionUser,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        IActionContextAccessor actionContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        IHttpContextAccessor httpContextAccessor,
        IGlobalSearch search,
        PermissionCheck permissionCheck): base(sessionUser) 
    {
        _search = search;
        _permissionCheck = permissionCheck;
        _sessionUser = sessionUser;
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _actionContextAccessor = actionContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
    }

    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public JsonResult GetPersonalWikiData(int id)
    {
        if (EntityCache.GetAllChildren(id).Any(c => c.Id == _sessionUser.User.StartTopicId))
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Category.LoopLink
            });

        var personalWiki = EntityCache.GetCategory(_sessionUser.User.StartTopicId);
        var personalWikiItem = new SearchHelper(_imageMetaDataReadingRepo,
                _actionContextAccessor,
                _httpContextAccessor,
                _webHostEnvironment)
            .FillSearchCategoryItem(personalWiki, UserId);
        var recentlyUsedRelationTargetTopics = new List<SearchCategoryItem>();

        if (_sessionUser.User.RecentlyUsedRelationTargetTopicIds != null && _sessionUser.User.RecentlyUsedRelationTargetTopicIds.Count > 0)
        {
            foreach (var topicId in _sessionUser.User.RecentlyUsedRelationTargetTopicIds)
            {
                var topicCacheItem = EntityCache.GetCategory(topicId);
                recentlyUsedRelationTargetTopics.Add(new SearchHelper(_imageMetaDataReadingRepo,
                    _actionContextAccessor,
                    _httpContextAccessor,
                    _webHostEnvironment)
                    .FillSearchCategoryItem(topicCacheItem, UserId));
            }
        }

        return Json(new RequestResult
        {
            success = true,
            data = new
            {
                personalWiki = personalWikiItem,
                recentlyUsedRelationTargetTopics = recentlyUsedRelationTargetTopics.ToArray()
            }
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult RemoveTopics(int parentId, int[] childIds)
    {
        return Json(null);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult AddToPersonalWiki(int id)
    {
        var personalWiki = EntityCache.GetCategory(_sessionUser.User.StartTopicId);

        if (personalWiki.DirectChildrenIds.Any(cId => cId == id))
        {
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Category.IsAlreadyLinkedAsChild
            });
        }

        var editTopicLogic =
            new EditControllerLogic(_search, _sessionUser.IsInstallationAdmin, _permissionCheck, _sessionUser);

        return Json(editTopicLogic.AddChild(id, personalWiki.Id));
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult RemoveFromPersonalWiki(int id)
    {
        var personalWiki = EntityCache.GetCategory(_sessionUser.User.StartTopicId);

        if (personalWiki.DirectChildrenIds.Any(cId => cId != id))
        {
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Category.IsAlreadyLinkedAsChild
            });
        }

        var editTopicLogic =
            new EditControllerLogic(_search, _sessionUser.IsInstallationAdmin, _permissionCheck, _sessionUser);

        return Json(editTopicLogic.RemoveParent(personalWiki.Id, id));
    }
}