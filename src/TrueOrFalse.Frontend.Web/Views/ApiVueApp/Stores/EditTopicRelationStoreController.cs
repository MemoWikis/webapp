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

    public EditTopicRelationStoreController(SessionUser sessionUser,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        IActionContextAccessor actionContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        IHttpContextAccessor httpContextAccessor) : base(sessionUser)
    {
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
            return Json(new
            {
                success = false,
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

        return Json(new
        {
            success = true,
            personalWiki = personalWikiItem,
            recentlyUsedRelationTargetTopics = recentlyUsedRelationTargetTopics.ToArray()
        });
    }
}