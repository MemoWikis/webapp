using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class EditTopicRelationStoreController : BaseController
{
    private readonly ImageMetaDataRepo _imageMetaDataRepo;

    public EditTopicRelationStoreController( SessionUser sessionUser, ImageMetaDataRepo imageMetaDataRepo) : base(sessionUser)
    {
        _imageMetaDataRepo = imageMetaDataRepo;
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
        var personalWikiItem = new SearchHelper(_imageMetaDataRepo).FillSearchCategoryItem(personalWiki,UserId);
        var recentlyUsedRelationTargetTopics = new List<SearchCategoryItem>();

        if (_sessionUser.User.RecentlyUsedRelationTargetTopicIds != null && _sessionUser.User.RecentlyUsedRelationTargetTopicIds.Count > 0)
        {
            foreach (var topicId in _sessionUser.User.RecentlyUsedRelationTargetTopicIds)
            {
                var topicCacheItem = EntityCache.GetCategory(topicId);
                recentlyUsedRelationTargetTopics.Add(new SearchHelper(_imageMetaDataRepo).FillSearchCategoryItem(topicCacheItem, UserId));
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