
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class TopicControllerLogic : IRegisterAsInstancePerLifetime
{
    private readonly SessionUser _sessionUser;
    private readonly PermissionCheck _permissionCheck;
    private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;
    private readonly CategoryViewRepo _categoryViewRepo;
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly SegmentationLogic _segmentationLogic;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly int _sessionUserId;

    public TopicControllerLogic(SessionUser sessionUser, 
        PermissionCheck permissionCheck,
        KnowledgeSummaryLoader knowledgeSummaryLoader,
        CategoryViewRepo categoryViewRepo,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        SegmentationLogic segmentationLogic,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        IActionContextAccessor actionContextAccessor)
    {
        _sessionUserId = sessionUser.UserId;
        _sessionUser = sessionUser;
        _permissionCheck = permissionCheck;
        _knowledgeSummaryLoader = knowledgeSummaryLoader;
        _categoryViewRepo = categoryViewRepo;
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _segmentationLogic = segmentationLogic;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _actionContextAccessor = actionContextAccessor;
    }

    public dynamic GetTopicData(int id)
    {
        var topic = EntityCache.GetCategory(id);

        if (_permissionCheck.CanView(_sessionUser.UserId, topic))
        {
            var imageMetaData = _imageMetaDataReadingRepo.GetBy(id, ImageType.Category);
            var knowledgeSummary = _knowledgeSummaryLoader.RunFromMemoryCache(id, _sessionUser.UserId);

            return new
            {
                CanAccess = true,
                Id = id,
                Name = topic.Name,
                ImageUrl = new CategoryImageSettings(id, _httpContextAccessor, _webHostEnvironment).GetUrl_128px(asSquare: true).Url,
                Content = topic.Content,
                ParentTopicCount = topic.ParentCategories().Where(_permissionCheck.CanView).ToList().Count,
                ChildTopicCount = topic.AggregatedCategories(_permissionCheck, false).Count,
                DirectChildTopicCount = topic.DirectChildrenIds.ToList().Count,
                Views = _categoryViewRepo.GetViewCount(id),
                Visibility = topic.Visibility,
                AuthorIds = topic.AuthorIds,
                Authors = topic.AuthorIds.Select(id =>
                {
                    var author = EntityCache.GetUserById(id);
                    return new
                    {
                        Id = id,
                        Name = author.Name,
                        ImgUrl = new UserImageSettings(author.Id, _httpContextAccessor, _webHostEnvironment).GetUrl_20px(author).Url,
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
                }
            };
        }

        return new { };
    }
    public dynamic GetTopicDataWithSegments(int id, ControllerContext context)
    {
        var topic = EntityCache.GetCategory(id);

        if (_permissionCheck.CanView(_sessionUser.UserId, topic))
        {
            var imageMetaData = _imageMetaDataReadingRepo.GetBy(id, ImageType.Category);
            var knowledgeSummary = _knowledgeSummaryLoader.RunFromMemoryCache(id, _sessionUser.UserId);
            return new
            {
                CanAccess = true,
                Id = id,
                Name = topic.Name,
                ImageUrl = new CategoryImageSettings(id, _httpContextAccessor, _webHostEnvironment).GetUrl_128px(asSquare: true).Url,
                Content = topic.Content,
                ParentTopicCount = topic.ParentCategories().Where(_permissionCheck.CanView).ToList().Count,
                ChildTopicCount = topic.AggregatedCategories(_permissionCheck, false).Count,
                DirectChildTopicCount = topic.DirectChildrenIds.Where(_permissionCheck.CanViewCategory).ToList().Count,
                Views = _categoryViewRepo.GetViewCount(id),
                Visibility = topic.Visibility,
                AuthorIds = topic.AuthorIds,
                Authors = topic.AuthorIds.Select(id =>
                {
                    var author = EntityCache.GetUserById(id);
                    return new
                    {
                        Id = id,
                        Name = author.Name,
                        ImgUrl = new UserImageSettings(author.Id, _httpContextAccessor, _webHostEnvironment).GetUrl_20px(author).Url,
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
                Segmentation = GetSegmentation(id, context)
            };
        }

        return new { };
    }

    private dynamic GetSegmentation(int id, ControllerContext context)
    {
       

        var category = EntityCache.GetCategory(id);
        var s = new SegmentationModel(category,_permissionCheck);
        var childTopics = _segmentationLogic.GetCategoriesData(s.NotInSegmentCategoryList.GetIds().ToArray());
        var segments = new List<dynamic>();
        if (s.Segments != null && s.Segments.Count > 0)
        {
            foreach (var segment in s.Segments)
            {
                var segmentChildrenIds = segment.ChildCategories.GetIds().ToArray();
                segments.Add(new
                {
                    Title = segment.Title,
                    CategoryId = segment.Item.Id,
                    ChildCategoryIds = segmentChildrenIds,
                    childTopics = _segmentationLogic.GetCategoriesData(segmentChildrenIds),
                    segmentData = _segmentationLogic.GetSegmentData(segment.Item.Id)
                });
            }
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
            Url = new Links(_actionContextAccessor, _httpContextAccessor)
                .CategoryDetail(topic.Name, topic.Id),
            QuestionCount = topic.GetCountQuestionsAggregated(_sessionUserId),
            ImageUrl = new CategoryImageSettings(topic.Id,
                    _httpContextAccessor, 
                    _webHostEnvironment)
                .GetUrl_128px(asSquare: true)
                .Url,
            MiniImageUrl = new ImageFrontendData(_imageMetaDataReadingRepo.GetBy(topic.Id, ImageType.Category),
                    _httpContextAccessor, 
                    _webHostEnvironment)
                .GetImageUrl(30, true, false, ImageType.Category).Url,
            Visibility = (int)topic.Visibility
        };

        return miniTopicItem;
    }
}

