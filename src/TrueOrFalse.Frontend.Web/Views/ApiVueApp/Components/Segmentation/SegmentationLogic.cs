using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;
public class SegmentationLogic : IRegisterAsInstancePerLifetime
{
    
    private readonly PermissionCheck _permissionCheck;
    private readonly int _sessionUserId;
    private readonly SessionUser _sessionUser;
    private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly SessionUserCache _sessionUserCache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public SegmentationLogic(
        PermissionCheck permissionCheck, 
        SessionUser sessionUser,
        KnowledgeSummaryLoader knowledgeSummaryLoader,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo, 
        SessionUserCache sessionUserCache,
        IHttpContextAccessor httpContextAccessor,
        IActionContextAccessor actionContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _permissionCheck = permissionCheck;
        _sessionUserId = sessionUser.UserId;
        _sessionUser = sessionUser;
        _knowledgeSummaryLoader = knowledgeSummaryLoader;
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _sessionUserCache = sessionUserCache;
        _httpContextAccessor = httpContextAccessor;
        _actionContextAccessor = actionContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }
    public dynamic GetSegmentation(int id)
    {
        var category = EntityCache.GetCategory(id);
        var s = new SegmentationModel(category,_permissionCheck);
        return new
        {
            childCategoryIds = s.NotInSegmentCategoryIds,
            segmentJson = s.SegmentJson
        };
    }
    public dynamic GetSegment(SegmentJson json)
    {
        var categoryId = json.CategoryId;
        var segment = new Segment();
        segment.Item = EntityCache.GetCategory(categoryId);

        if (json.ChildCategoryIds != null)
            segment.ChildCategories = EntityCache.GetCategories(json.ChildCategoryIds).ToList();
        else
            segment.ChildCategories = EntityCache.GetChildren(categoryId);

        return new
        {
            CategoryId = segment.Item.Id,
            Title = json.Title,
            ChildCategoryIds = "[" + String.Join(", ", segment.ChildCategories.Select(c => c.Id).ToList()) + "]",
        };
    }

    public dynamic GetCategoriesData(int[] categoryIds)
    {
        var ids = categoryIds;
        ConcurrentDictionary<int, CategoryValuation> userValuation = null;
        var startTopicId = 1;

        if (_sessionUser.IsLoggedIn)
        {
            userValuation = _sessionUserCache.GetItem(_sessionUser.UserId).CategoryValuations;
            startTopicId = _sessionUserCache.GetUser(_sessionUser.UserId).StartTopicId;
        }

        var categoryDataList = categoryIds.Select(
            categoryId => _sessionUser.IsLoggedIn ? GetCategoryCardData(categoryId, userValuation, startTopicId)
                : GetCategoryCardData(categoryId)).Where(categoryCardData => categoryCardData != null)
            .ToList();

        return categoryDataList;
    }

    public dynamic GetCategoryData(int categoryId)
    {
        var categoryCardData = _sessionUser.IsLoggedIn
            ? GetCategoryCardData(categoryId, _sessionUserCache.GetItem(_sessionUserId).CategoryValuations,
                _sessionUserCache.GetUser(_sessionUser.UserId).StartTopicId)
            : GetCategoryCardData(categoryId);
        return categoryCardData != null ? categoryCardData : "";
    }

    private CategoryCardData GetCategoryCardData(int categoryId, ConcurrentDictionary<int, CategoryValuation> userValuation = null, int? startTopicId = null)
    {
        var categoryCacheItem = EntityCache.GetCategory(categoryId);
        if (!_permissionCheck.CanView(categoryCacheItem)) return null;

        var linkToCategory = new Links(_actionContextAccessor, _httpContextAccessor).CategoryDetail(categoryCacheItem);

        var imageMetaData = _imageMetaDataReadingRepo.GetBy(categoryId, ImageType.Category);
        var imageFrontendData = new ImageFrontendData(imageMetaData, _httpContextAccessor, _webHostEnvironment);
        var imgHtml = imageFrontendData.RenderHtmlImageBasis(128, true, ImageType.Category);
        var imgUrl = imageFrontendData.GetImageUrl(128, true, false, ImageType.Category).Url;

        var childCategoryCount = EntityCache.GetChildren(categoryId).Where(_permissionCheck.CanView).Distinct().Count();
        var questionCount = categoryCacheItem.GetAggregatedQuestionsFromMemoryCache(_sessionUserId).Count;

        var knowledgeBarSummary = new CategoryKnowledgeBarModel(categoryCacheItem,_sessionUserId, _knowledgeSummaryLoader).CategoryKnowledgeSummary;

        var isInWishknowledge = false;
        var isPersonalHomepage = false;
        if (_sessionUser.IsLoggedIn)
        {
            if (userValuation != null && userValuation.ContainsKey(categoryId))
                isInWishknowledge = userValuation[categoryId].IsInWishKnowledge();
            isPersonalHomepage = categoryCacheItem.Id == startTopicId;
        }

        return new CategoryCardData
        {
            Id = categoryCacheItem.Id,
            Name = categoryCacheItem.Name,
            Visibility = (int)categoryCacheItem.Visibility,
            LinkToCategory = linkToCategory,
            ImgUrl = imgUrl,
            ImgHtml = imgHtml,
            KnowledgeBarData =
            new {
                Total = knowledgeBarSummary.Total,
                NeedsLearning = knowledgeBarSummary.NeedsLearning,
                NeedsLearningPercentage = knowledgeBarSummary.NeedsLearningPercentage,
                NeedsConsolidation = knowledgeBarSummary.NeedsConsolidation,
                NeedsConsolidationPercentage = knowledgeBarSummary.NeedsConsolidationPercentage,
                Solid = knowledgeBarSummary.Solid,
                SolidPercentage = knowledgeBarSummary.SolidPercentage,
                NotLearned = knowledgeBarSummary.NotLearned,
                NotLearnedPercentage = knowledgeBarSummary.NotLearnedPercentage
            },
            ChildCategoryCount = childCategoryCount,
            QuestionCount = questionCount,
            IsInWishknowledge = isInWishknowledge,
            IsPersonalHomepage = isPersonalHomepage
        };

    }

    private class CategoryCardData
    {
        public int Id;
        public string Name;
        public int Visibility;
        public string LinkToCategory;
        public string ImgHtml;
        public dynamic KnowledgeBarData;
        public int ChildCategoryCount;
        public int QuestionCount;
        public bool IsInWishknowledge = false;
        public bool IsPersonalHomepage = false;
        public string ImgUrl;
    }

    public dynamic GetSegmentData(int categoryId)
    {
        var categoryCacheItem = EntityCache.GetCategory(categoryId);
        var linkToCategory = new Links(_actionContextAccessor, _httpContextAccessor).CategoryDetail(categoryCacheItem);
        var knowledgeBarSummary = _knowledgeSummaryLoader.RunFromMemoryCache(categoryId, _sessionUserId);

        return new
        {
            categoryId = categoryCacheItem.Id,
            categoryName = categoryCacheItem.Name,
            visibility = categoryCacheItem.Visibility,
            linkToCategory,
            knowledgeBarData = new
            {
                Total = knowledgeBarSummary.Total,
                NeedsLearning = knowledgeBarSummary.NeedsLearning,
                NeedsLearningPercentage = knowledgeBarSummary.NeedsLearningPercentage,
                NeedsConsolidation = knowledgeBarSummary.NeedsConsolidation,
                NeedsConsolidationPercentage = knowledgeBarSummary.NeedsConsolidationPercentage,
                Solid = knowledgeBarSummary.Solid,
                SolidPercentage = knowledgeBarSummary.SolidPercentage,
                NotLearned = knowledgeBarSummary.NotLearned,
                NotLearnedPercentage = knowledgeBarSummary.NotLearnedPercentage
            },
        };
    }
}






