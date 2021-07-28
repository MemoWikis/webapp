using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class SegmentationController : BaseController
{
    [HttpPost]
    public JsonResult GetSegment(SegmentJson json)
    {
        var categoryId = json.CategoryId;
        var segment = new Segment();
        segment.Item = EntityCache.GetCategoryCacheItem(categoryId);

        if (json.ChildCategoryIds != null)
            segment.ChildCategories = UserCache.GetItem(_sessionUser.UserId).IsFiltered ? EntityCache.GetCategoryCacheItems(json.ChildCategoryIds)
                .Where(c => c.IsInWishknowledge()).ToList() : EntityCache.GetCategoryCacheItems(json.ChildCategoryIds).ToList();
        else
            segment.ChildCategories = UserCache.GetItem(_sessionUser.UserId).IsFiltered ? UserEntityCache.GetChildren(categoryId, UserId) :  EntityCache.GetChildren(categoryId);

        return Json(new
        {
            CategoryId = segment.Item.Id,
            Title = json.Title,
            ChildCategoryIds = "[" + String.Join(", ", segment.ChildCategories.Select(c => c.Id).ToList()) + "]",
        });
    }

    [HttpPost]
    public JsonResult GetCategoryCard(int categoryId)
    {
        var category = EntityCache.GetCategoryCacheItem(categoryId);
        var categoryCardHtml = ViewRenderer.RenderPartialView(
            "~/Views/Categories/Detail/Partials/Segmentation/SegmentationCategoryCardComponent.vue.ascx", new SegmentationCategoryCardModel(category),
            ControllerContext);

        return Json(new
        {
            html = categoryCardHtml
        });
    }

    [HttpPost]
    public JsonResult GetCategoryData(int categoryId)
    {
        var categoryCardData = IsLoggedIn ? GetCategoryCardData(categoryId, UserCache.GetItem(UserId).CategoryValuations) : GetCategoryCardData(categoryId);
        return Json(categoryCardData);
    }

    [HttpPost]
    public JsonResult GetCategoriesData(int[] categoryIds)
    {
        ConcurrentDictionary<int, CategoryValuation> userValuation = null;

        if (IsLoggedIn)
            userValuation = UserCache.GetItem(UserId).CategoryValuations;
        
        var categoryDataList = new List<CategoryCardData>();
        foreach (int categoryId in categoryIds)
        {
            CategoryCardData categoryCardData;

            if (IsLoggedIn)
                categoryCardData = GetCategoryCardData(categoryId, userValuation);
            else
                categoryCardData = GetCategoryCardData(categoryId);

            categoryDataList.Add(categoryCardData);
        }

        return Json(categoryDataList);
    }

    private CategoryCardData GetCategoryCardData(int categoryId, ConcurrentDictionary<int, CategoryValuation> userValuation = null)
    {
        var categoryCacheItem = EntityCache.GetCategoryCacheItem(categoryId);

        var linkToCategory = Links.CategoryDetail(categoryCacheItem);
        var categoryTypeHtml = categoryCacheItem.Type.GetCategoryTypeIconHtml();

        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(categoryId, ImageType.Category);
        var imageFrontendData = new ImageFrontendData(imageMetaData);
        var imgHtml = imageFrontendData.RenderHtmlImageBasis(128, true, ImageType.Category);

        var childCategoryCount = categoryCacheItem.CachedData.ChildrenIds.Count;
        var questionCount = categoryCacheItem.GetAggregatedQuestionsFromMemoryCache().Count;
        var knowledgeBarHtml = "";
        if (questionCount > 0)
            knowledgeBarHtml = ViewRenderer.RenderPartialView("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(categoryCacheItem), ControllerContext);

        var isInWishknowledge = false;
        if (IsLoggedIn)
            if (userValuation.ContainsKey(categoryId))
                isInWishknowledge = userValuation[categoryId].IsInWishKnowledge();

        return new CategoryCardData
        {
            Id = categoryCacheItem.Id,
            Name = categoryCacheItem.Name,
            Visibility = (int)categoryCacheItem.Visibility,
            LinkToCategory = linkToCategory,
            CategoryTypeHtml = categoryTypeHtml,
            ImgHtml = imgHtml,
            KnowledgeBarHtml = knowledgeBarHtml,
            ChildCategoryCount = childCategoryCount,
            QuestionCount = questionCount,
            IsInWishknowledge = isInWishknowledge
        };
    }

    private class CategoryCardData
    {
        public int Id;
        public string Name;
        public int Visibility;
        public string LinkToCategory;
        public string CategoryTypeHtml;
        public string ImgHtml;
        public string KnowledgeBarHtml;
        public int ChildCategoryCount;
        public int QuestionCount;
        public bool IsInWishknowledge = false;
    }

    [HttpPost]
    public JsonResult GetSegmentData(int categoryId)
    {
        var categoryCacheItem = EntityCache.GetCategoryCacheItem(categoryId);
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






