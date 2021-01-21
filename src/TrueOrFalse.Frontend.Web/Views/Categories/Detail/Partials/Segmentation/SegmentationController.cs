using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;

public class SegmentationController : BaseController
{
    [HttpPost]
    public JsonResult GetSegmentHtml(int categoryId, int[] childCategoryIds)
    {
        var segment = new Segment();
        segment.Category = EntityCache.GetCategory(categoryId);
        foreach (int childCategoryId in childCategoryIds)
            segment.ChildCategories.Add(EntityCache.GetCategory(childCategoryId));

        var segmentHtml = ViewRenderer.RenderPartialView(
            "~/Views/Categories/Detail/Partials/Segmentation/SegmentComponent.vue.ascx", new SegmentModel(segment),
            ControllerContext);

        return Json(new
        {
            html = segmentHtml
        });
    }
}






