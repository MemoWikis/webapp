using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentNHibernate.Data;
using TrueOrFalse.Frontend.Web.Code;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class SegmentationController : BaseController
{

    [HttpPost]
    public JsonResult GetSegmentation(int id)
    {
        var category = EntityCache.GetCategory(id);
        var s = new SegmentationModel(category);
        return Json(new
        {
            childCategoryIds = s.NotInSegmentCategoryIds,
            segmentJson = s.SegmentJson
        });
    }
    [HttpPost]
    public JsonResult GetSegment(SegmentJson json)
    {
        var categoryId = json.CategoryId;
        var segment = new Segment();
        segment.Item = EntityCache.GetCategory(categoryId);

        if (json.ChildCategoryIds != null)
            segment.ChildCategories = EntityCache.GetCategories(json.ChildCategoryIds).ToList();
        else
            segment.ChildCategories = EntityCache.GetChildren(categoryId);

        return Json(new
        {
            CategoryId = segment.Item.Id,
            Title = json.Title, 
            ChildCategoryIds = "[" + String.Join(", ", segment.ChildCategories.Select(c => c.Id).ToList()) + "]",
        });
    }

    [HttpPost]
    public JsonResult GetCategoriesData(int[] categoryIds)
    {
        ConcurrentDictionary<int, CategoryValuation> userValuation = null;
        var startTopicId = 0;

        if (IsLoggedIn)
        {
            userValuation = SessionUserCache.GetItem(UserId).CategoryValuations;
            startTopicId = SessionUserCache.GetUser(UserId).StartTopicId;
        }

        var categoryDataList = categoryIds.Select(
            categoryId => IsLoggedIn ? GetCategoryCardData(categoryId, userValuation, startTopicId)
                : GetCategoryCardData(categoryId)).Where(categoryCardData => categoryCardData != null)
            .ToArray();

        return Json(categoryDataList);
    }

    [HttpPost]
    public JsonResult GetCategoryData(int categoryId)
    {
        var categoryCardData = IsLoggedIn ? GetCategoryCardData(categoryId, SessionUserCache.GetItem(UserId).CategoryValuations, SessionUserCache.GetUser(UserId).StartTopicId) : GetCategoryCardData(categoryId);
        return categoryCardData != null ? Json(categoryCardData) : Json("");
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
        var knowledgeBarHtml = "";
        if (questionCount > 0)
            knowledgeBarHtml = ViewRenderer.RenderPartialView("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx",
                new CategoryKnowledgeBarModel(categoryCacheItem), ControllerContext);

        var isInWishknowledge = false;
        var isPersonalHomepage = false;
        if (IsLoggedIn)
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
            ImgHtml = imgHtml,
            KnowledgeBarHtml = knowledgeBarHtml,
            ChildCategoryCount = childCategoryCount,
            QuestionCount = questionCount,
            IsInWishknowledge = isInWishknowledge,
            IsPersonalHomepage = isPersonalHomepage,
            ImgUrl = imgUrl
        };

    }

    private class CategoryCardData
    {
        public int Id;
        public string Name;
        public int Visibility;
        public string LinkToCategory;
        public string ImgHtml;
        public string KnowledgeBarHtml;
        public int ChildCategoryCount;
        public int QuestionCount;
        public bool IsInWishknowledge = false;
        public bool IsPersonalHomepage = false;
        public string ImgUrl;
    }

    [HttpPost]
    public JsonResult GetSegmentData(int categoryId)
    {
        var categoryCacheItem = EntityCache.GetCategory(categoryId);
        var linkToCategory = Links.CategoryDetail(categoryCacheItem);

        var questionCount = categoryCacheItem.GetAggregatedQuestionsFromMemoryCache().Count;
        var knowledgeBarHtml = "";
        if (questionCount > 0)
            knowledgeBarHtml = ViewRenderer.RenderPartialView("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(categoryCacheItem), ControllerContext);
        return Json(new
        {
            categoryId = categoryCacheItem.Id,
            categoryName = categoryCacheItem.Name,
            visibility = categoryCacheItem.Visibility,
            linkToCategory,
            knowledgeBarHtml
        });
    }
}






