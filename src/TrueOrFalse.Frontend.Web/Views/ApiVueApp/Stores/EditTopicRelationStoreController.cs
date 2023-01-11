using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class EditTopicRelationStoreController
    : BaseController
{
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult GetPersonalWikiData(int id)
    {

        if (EntityCache.GetAllChildren(id).Any(c => c.Id == SessionUser.User.StartTopicId))
            return Json(new
            {
                success = false,
            });

        var personalWiki = EntityCache.GetCategory(SessionUser.User.StartTopicId);
        var personalWikiItem = FillSearchCategoryItem(personalWiki);
        var recentlyUsedRelationTargetTopics = new List<SearchCategoryItem>();

        if (SessionUser.User.RecentlyUsedRelationTargetTopicIds != null && SessionUser.User.RecentlyUsedRelationTargetTopicIds.Count > 0)
        {
            foreach (var categoryId in SessionUser.User.RecentlyUsedRelationTargetTopicIds)
            {
                var c = EntityCache.GetCategory(categoryId);
                recentlyUsedRelationTargetTopics.Add(FillSearchCategoryItem(c));
            }
        }

        return Json(new
        {
            success = true,
            personalWiki = personalWikiItem,
            recentlyUsedRelationTargetTopics = recentlyUsedRelationTargetTopics.ToArray()
        });
    }

    public static SearchCategoryItem FillSearchCategoryItem(CategoryCacheItem c)
    {
        return new SearchCategoryItem
        {
            Id = c.Id,
            Name = c.Name,
            Url = Links.CategoryDetail(c.Name, c.Id),
            QuestionCount = EntityCache.GetCategory(c.Id).GetCountQuestionsAggregated(),
            ImageUrl = new CategoryImageSettings(c.Id).GetUrl_128px(asSquare: true).Url,
            //IconHtml = GetIconHtml(c),
            MiniImageUrl = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(c.Id, ImageType.Category))
                .GetImageUrl(30, true, false, ImageType.Category).Url,
            Visibility = (int)c.Visibility
        };
    }


}