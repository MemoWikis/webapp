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
        segment.Category = EntityCache.GetCategory(categoryId);
        if (json.ChildCategoryIds != null)
            foreach (int childCategoryId in json.ChildCategoryIds)
                segment.ChildCategories.Add(EntityCache.GetCategory(childCategoryId));
        else
            segment.ChildCategories = EntityCache.GetChildren(categoryId);
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
        var category = EntityCache.GetCategory(categoryId);
        var categoryCardHtml = ViewRenderer.RenderPartialView(
            "~/Views/Categories/Detail/Partials/Segmentation/SegmentationCategoryCardComponent.vue.ascx", new SegmentationCategoryCardModel(category),
            ControllerContext);

        return Json(new
        {
            html = categoryCardHtml
        });
    }
}






