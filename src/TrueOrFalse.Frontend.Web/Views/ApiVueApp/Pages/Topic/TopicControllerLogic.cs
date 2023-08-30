
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

namespace VueApp;

public class TopicControllerLogic : IRegisterAsInstancePerLifetime
{
    private readonly SessionUser _sessionUser;
    private readonly PermissionCheck _permissionCheck;
    private readonly int _sessionUserId;

    public TopicControllerLogic(SessionUser sessionUser, 
        PermissionCheck permissionCheck, GridItemLogic gridItemLogic)
    {
        _sessionUserId = sessionUser.UserId;
        _sessionUser = sessionUser;
        _permissionCheck = permissionCheck;
    }

    private dynamic CreateTopicDataObject(int id, CategoryCacheItem topic, ImageMetaData imageMetaData, KnowledgeSummary knowledgeSummary)
    {
        var gridItemLogic = new GridItemLogic(_permissionCheck, _sessionUser);
        return new
        {
            CanAccess = true,
            Id = id,
            Name = topic.Name,
            ImageUrl = new CategoryImageSettings(id).GetUrl_128px(asSquare: true).Url,
            Content = topic.Content,
            ParentTopicCount = topic.ParentCategories().Where(_permissionCheck.CanView).ToList().Count,
            Parents = topic.ParentCategories().Where(_permissionCheck.CanView).Select(p => new { id = p.Id, name = p.Name, imgUrl = new CategoryImageSettings(p.Id).GetUrl(50, true).Url }).ToArray(),
            ChildTopicCount = topic.AggregatedCategories(_permissionCheck, false).Count,
            DirectChildTopicCount = topic.DirectChildrenIds.Where(_permissionCheck.CanViewCategory).ToList().Count,
            Views = Sl.CategoryViewRepo.GetViewCount(id),
            Visibility = topic.Visibility,
            AuthorIds = topic.AuthorIds,
            Authors = topic.AuthorIds.Select(id =>
            {
                var author = EntityCache.GetUserById(id);
                return new
                {
                    Id = id,
                    Name = author.Name,
                    ImgUrl = new UserImageSettings(author.Id).GetUrl_20px(author).Url,
                    Reputation = author.Reputation,
                    ReputationPos = author.ReputationPos
                };
            }).ToArray(),
            IsWiki = topic.IsStartPage(),
            CurrentUserIsCreator = _sessionUser.User != null && _sessionUser.UserId == topic.Creator?.Id,
            CanBeDeleted = _sessionUser.User != null && _permissionCheck.CanDelete(topic),
            QuestionCount = topic.GetAggregatedQuestionsFromMemoryCache(_sessionUserId).Count,
            DirectQuestionCount = topic.GetCountQuestionsAggregated(_sessionUserId, true),
            ImageId = imageMetaData != null ? imageMetaData.Id : 0,
            SearchTopicItem = FillMiniTopicItem(topic),
            MetaDescription = SeoUtils.ReplaceDoubleQuotes(topic.Content == null ? null : Regex.Replace(topic.Content, "<.*?>", "")).Truncate(250, true),
            KnowledgeSummary = new
            {
                notLearned = knowledgeSummary.NotLearned + knowledgeSummary.NotInWishknowledge,
                needsLearning = knowledgeSummary.NeedsLearning,
                needsConsolidation = knowledgeSummary.NeedsConsolidation,
                solid = knowledgeSummary.Solid
            },
            gridItems = gridItemLogic.GetChildren(id),
        };
    }


    public dynamic GetTopicData(int id)
    {
        var topic = EntityCache.GetCategory(id);

        if (_permissionCheck.CanView(_sessionUser.UserId, topic))
        {
            var imageMetaData = Sl.ImageMetaDataRepo.GetBy(id, ImageType.Category);
            var knowledgeSummary = KnowledgeSummaryLoader.RunFromMemoryCache(id, _sessionUser.UserId);

            return CreateTopicDataObject(id, topic, imageMetaData, knowledgeSummary);
        }

        return new { };
    }

    private SearchTopicItem FillMiniTopicItem(CategoryCacheItem topic)
    {
        var miniTopicItem = new SearchTopicItem
        {
            Id = topic.Id,
            Name = topic.Name,
            Url = Links.CategoryDetail(topic.Name, topic.Id),
            QuestionCount = topic.GetCountQuestionsAggregated(_sessionUserId),
            ImageUrl = new CategoryImageSettings(topic.Id).GetUrl_128px(asSquare: true).Url,
            MiniImageUrl = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(topic.Id, ImageType.Category))
                .GetImageUrl(30, true, false, ImageType.Category).Url,
            Visibility = (int)topic.Visibility
        };

        return miniTopicItem;
    }
}

