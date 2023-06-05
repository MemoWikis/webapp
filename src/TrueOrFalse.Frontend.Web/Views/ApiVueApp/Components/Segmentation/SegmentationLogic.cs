using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentNHibernate.Data;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;
[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class SegmentationLogic
{
    private ControllerContext _controllerContext;
    public SegmentationLogic(ControllerContext controllerContext)
    {
        _controllerContext = controllerContext;
    }
    public dynamic GetSegmentation(int id)
    {
        var category = EntityCache.GetCategory(id);
        var s = new SegmentationModel(category);
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

        if (SessionUserLegacy.IsLoggedIn)
        {
            userValuation = SessionUserCache.GetItem(SessionUserLegacy.UserId).CategoryValuations;
            startTopicId = SessionUserCache.GetUser(SessionUserLegacy.UserId).StartTopicId;
        }

        var categoryDataList = categoryIds.Select(
            categoryId => SessionUserLegacy.IsLoggedIn ? GetCategoryCardData(categoryId, userValuation, startTopicId)
                : GetCategoryCardData(categoryId)).Where(categoryCardData => categoryCardData != null)
            .ToList();

        return categoryDataList;
    }

    public dynamic GetCategoryData(int categoryId)
    {
        var categoryCardData = SessionUserLegacy.IsLoggedIn
            ? GetCategoryCardData(categoryId, SessionUserCache.GetItem(SessionUserLegacy.UserId).CategoryValuations,
                SessionUserCache.GetUser(SessionUserLegacy.UserId).StartTopicId)
            : GetCategoryCardData(categoryId);
        return categoryCardData != null ? categoryCardData : "";
    }

    private CategoryCardData GetCategoryCardData(int categoryId, ConcurrentDictionary<int, CategoryValuation> userValuation = null, int? startTopicId = null)
    {
        var categoryCacheItem = EntityCache.GetCategory(categoryId);
        if (!PermissionCheck.CanView(categoryCacheItem)) return null;

        var linkToCategory = Links.CategoryDetail(categoryCacheItem);

        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(categoryId, ImageType.Category);
        var imageFrontendData = new ImageFrontendData(imageMetaData);
        var imgHtml = imageFrontendData.RenderHtmlImageBasis(128, true, ImageType.Category);
        var imgUrl = imageFrontendData.GetImageUrl(128, true, false, ImageType.Category).Url;

        var childCategoryCount = EntityCache.GetChildren(categoryId).Where(PermissionCheck.CanView).Distinct().Count();
        var questionCount = categoryCacheItem.GetAggregatedQuestionsFromMemoryCache().Count;

        var knowledgeBarSummary = new CategoryKnowledgeBarModel(categoryCacheItem).CategoryKnowledgeSummary;

        var isInWishknowledge = false;
        var isPersonalHomepage = false;
        if (SessionUserLegacy.IsLoggedIn)
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
        var linkToCategory = Links.CategoryDetail(categoryCacheItem);

        var questionCount = categoryCacheItem.GetAggregatedQuestionsFromMemoryCache().Count;
        var knowledgeBarHtml = "";
        if (questionCount > 0)
            knowledgeBarHtml = ViewRenderer.RenderPartialView("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(categoryCacheItem), _controllerContext);
        return new
        {
            categoryId = categoryCacheItem.Id,
            categoryName = categoryCacheItem.Name,
            visibility = categoryCacheItem.Visibility,
            linkToCategory,
            knowledgeBarHtml
        };
    }
}






