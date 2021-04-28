using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using TrueOrFalse.Frontend.Web.Code;

public class SegmentationController : BaseController
{
    [HttpPost]
    public JsonResult GetSegmentHtml(SegmentJson json)
 
    {
        var categoryId = json.CategoryId;
        var segment = new Segment();
        segment.Item = EntityCache.GetCategoryCacheItem(categoryId);

        if (json.ChildCategoryIds != null)
            segment.ChildCategories = UserCache.GetItem(_sessionUser.UserId).IsFiltered ? EntityCache.GetCategoryCacheItems(json.ChildCategoryIds)
                .Where(c => c.IsInWishknowledge()).ToList() : EntityCache.GetCategoryCacheItems(json.ChildCategoryIds).ToList();
        else
            segment.ChildCategories = UserCache.GetItem(_sessionUser.UserId).IsFiltered ? UserEntityCache.GetChildren(categoryId, UserId) :  EntityCache.GetChildren(categoryId);

        var segmentHtml = ViewRenderer.RenderPartialView(
            "~/Views/Categories/Detail/Partials/Segmentation/SegmentComponent.vue.ascx", new SegmentModel(segment),
            ControllerContext);

        return Json(new
        {
            html = segmentHtml
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

        return Json(new
        {
            categoryName = categoryCacheItem.Name,
            visibility = categoryCacheItem.Visibility,
            linkToCategory,
            categoryTypeHtml,
            imgHtml,
            childCategoryCount,
            questionCount,
            knowledgeBarHtml
        });
    }
}






