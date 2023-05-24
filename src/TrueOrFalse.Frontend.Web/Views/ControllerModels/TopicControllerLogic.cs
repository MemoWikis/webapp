
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Newtonsoft.Json;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

namespace VueApp;

public class TopicControllerLogic 
{
    public dynamic GetTopicData(int id)
    {
        var topic = EntityCache.GetCategory(id);

        if (PermissionCheck.CanView(topic))
        {
            var imageMetaData = Sl.ImageMetaDataRepo.GetBy(id, ImageType.Category);
            var knowledgeSummary = KnowledgeSummaryLoader.RunFromMemoryCache(id, SessionUser.UserId);

            return new
            {
                CanAccess = true,
                Id = id,
                Name = topic.Name,
                ImageUrl = new CategoryImageSettings(id).GetUrl_128px(asSquare: true).Url,
                Content = topic.Content,
                ParentTopicCount = topic.ParentCategories().Count,
                ChildTopicCount = topic.AggregatedCategories(false).Count,
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
                CurrentUserIsCreator = SessionUser.User != null && SessionUser.UserId == topic.Creator?.Id,
                CanBeDeleted = SessionUser.User != null && PermissionCheck.CanDelete(topic),
                QuestionCount = topic.GetAggregatedQuestionsFromMemoryCache().Count,
                ImageId = imageMetaData != null ? imageMetaData.Id : 0,
                EncodedName = UriSanitizer.Run(topic.Name),
                SearchTopicItem = FillMiniTopicItem(topic),
                MetaDescription = SeoUtils.ReplaceDoubleQuotes(topic.Content == null ? null : Regex.Replace(topic.Content, "<.*?>", "")).Truncate(250, true),
                KnowledgeSummary = new
                {
                    notLearned = knowledgeSummary.NotLearned + knowledgeSummary.NotInWishknowledge,
                    needsLearning = knowledgeSummary.NeedsLearning,
                    needsConsolidation = knowledgeSummary.NeedsConsolidation,
                    solid = knowledgeSummary.Solid
                }
            };
        }

        return new { };
    }
    public dynamic GetTopicDataWithSegments(int id, ControllerContext context)
    {
        var topic = EntityCache.GetCategory(id);

        if (PermissionCheck.CanView(topic))
        {
            var imageMetaData = Sl.ImageMetaDataRepo.GetBy(id, ImageType.Category);
            var knowledgeSummary = KnowledgeSummaryLoader.RunFromMemoryCache(id, SessionUser.UserId);
            return new
            {
                CanAccess = true,
                Id = id,
                Name = topic.Name,
                ImageUrl = new CategoryImageSettings(id).GetUrl_128px(asSquare: true).Url,
                Content = topic.Content,
                ParentTopicCount = topic.ParentCategories().Count,
                ChildTopicCount = topic.AggregatedCategories(false).Count,
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
                CurrentUserIsCreator = SessionUser.User != null && SessionUser.UserId == topic.Creator?.Id,
                CanBeDeleted = SessionUser.User != null && PermissionCheck.CanDelete(topic),
                QuestionCount = topic.GetAggregatedQuestionsFromMemoryCache().Count,
                ImageId = imageMetaData != null ? imageMetaData.Id : 0,
                EncodedName = UriSanitizer.Run(topic.Name),
                SearchTopicItem = FillMiniTopicItem(topic),
                MetaDescription = SeoUtils.ReplaceDoubleQuotes(topic.Content == null ? null : Regex.Replace(topic.Content, "<.*?>", "")).Truncate(250, true),
                KnowledgeSummary = new
                {
                    notLearned = knowledgeSummary.NotLearned + knowledgeSummary.NotInWishknowledge,
                    needsLearning = knowledgeSummary.NeedsLearning,
                    needsConsolidation = knowledgeSummary.NeedsConsolidation,
                    solid = knowledgeSummary.Solid
                },
                Segmentation = GetSegmentation(id, context)
            };
        }

        return new { };
    }

    private dynamic GetSegmentation(int id, ControllerContext context)
    {
        var segmentationLogic = new SegmentationLogic(context);

        var category = EntityCache.GetCategory(id);
        var s = new SegmentationModel(category);
        var childTopics = segmentationLogic.GetCategoriesData(s.NotInSegmentCategoryList.GetIds().ToArray());
        var segments = new List<dynamic>();
        foreach (var segment in s.Segments)
        {
            var segmentChildrenIds = segment.ChildCategories.GetIds().ToArray();
            segments.Add(new 
            {
                Title = segment.Title,
                CategoryId = segment.Item.Id,
                ChildCategoryIds = segmentChildrenIds,
                childTopics = segmentationLogic.GetCategoriesData(segmentChildrenIds),
                segmentData = segmentationLogic.GetSegmentData(segment.Item.Id)
            });
        }
        return new
        {
            childCategoryIds = s.NotInSegmentCategoryIds,
            segmentJson = s.SegmentJson,
            childTopics = childTopics,
            segments = segments
        };
    }

    private SearchTopicItem FillMiniTopicItem(CategoryCacheItem topic)
    {
        var miniTopicItem = new SearchTopicItem
        {
            Id = topic.Id,
            Name = topic.Name,
            Url = Links.CategoryDetail(topic.Name, topic.Id),
            QuestionCount = topic.GetCountQuestionsAggregated(),
            ImageUrl = new CategoryImageSettings(topic.Id).GetUrl_128px(asSquare: true).Url,
            MiniImageUrl = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(topic.Id, ImageType.Category))
                .GetImageUrl(30, true, false, ImageType.Category).Url,
            Visibility = (int)topic.Visibility
        };

        return miniTopicItem;
    }
}

