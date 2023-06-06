using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class EditTopicRelationStoreController : BaseController
{
    public EditTopicRelationStoreController( SessionUser sessionUser) : base(sessionUser)
    {
        
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
        var personalWikiItem = SearchHelper.FillSearchCategoryItem(personalWiki);
        var recentlyUsedRelationTargetTopics = new List<SearchCategoryItem>();

        if (_sessionUser.User.RecentlyUsedRelationTargetTopicIds != null && _sessionUser.User.RecentlyUsedRelationTargetTopicIds.Count > 0)
        {
            foreach (var topicId in _sessionUser.User.RecentlyUsedRelationTargetTopicIds)
            {
                var topicCacheItem = EntityCache.GetCategory(topicId);
                recentlyUsedRelationTargetTopics.Add(SearchHelper.FillSearchCategoryItem(topicCacheItem));
            }
        }

        return Json(new
        {
            success = true,
            personalWiki = personalWikiItem,
            recentlyUsedRelationTargetTopics = recentlyUsedRelationTargetTopics.ToArray()
        }, JsonRequestBehavior.AllowGet);
    }
}