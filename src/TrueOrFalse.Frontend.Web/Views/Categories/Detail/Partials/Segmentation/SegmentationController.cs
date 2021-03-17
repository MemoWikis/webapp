using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;

public class SegmentationController : BaseController
{
    [HttpPost]
    public JsonResult GetSegmentHtml(SegmentJson json)
    {
        var categoryId = json.CategoryId;
        var segment = new Segment();
        segment.Item = EntityCache.GetCategoryCacheItem(categoryId);
        if (json.ChildCategoryIds != null)
            segment.ChildCategories = UserCache.GetItem(_sessionUser.UserId).IsFiltered ? EntityCache.GetCategories(json.ChildCategoryIds).Where(c => c.IsInWishknowledge()).ToList() : EntityCache.GetCategories(json.ChildCategoryIds).ToList();
        else
            segment.ChildCategories = UserCache.GetItem(_sessionUser.UserId).IsFiltered ? UserEntityCache.GetChildren(categoryId, UserId) : Sl.CategoryRepo.GetChildren(categoryId).ToList();

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
}






