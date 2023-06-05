using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class EditTopicRelationStoreController
    : BaseController
{
    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public JsonResult GetPersonalWikiData(int id)
    {
        if (EntityCache.GetAllChildren(id).Any(c => c.Id == SessionUserLegacy.User.StartTopicId))
            return Json(new
            {
                success = false,
            });

        var personalWiki = EntityCache.GetCategory(SessionUserLegacy.User.StartTopicId);
        var personalWikiItem = SearchHelper.FillSearchCategoryItem(personalWiki);
        var recentlyUsedRelationTargetTopics = new List<SearchCategoryItem>();

        if (SessionUserLegacy.User.RecentlyUsedRelationTargetTopicIds != null && SessionUserLegacy.User.RecentlyUsedRelationTargetTopicIds.Count > 0)
        {
            foreach (var topicId in SessionUserLegacy.User.RecentlyUsedRelationTargetTopicIds)
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