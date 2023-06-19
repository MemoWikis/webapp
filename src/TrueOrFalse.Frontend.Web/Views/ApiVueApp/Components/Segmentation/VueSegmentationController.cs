using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentNHibernate.Data;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;
[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class VueSegmentationController : Controller
{
    private readonly PermissionCheck _permissionCheck;

    public VueSegmentationController(PermissionCheck permissionCheck)
    {
        _permissionCheck = permissionCheck;
    }
    [HttpPost]
    public JsonResult GetSegmentation(int id)
    {
        var segmentationLogic = new SegmentationLogic(ControllerContext,_permissionCheck);
        return Json(segmentationLogic.GetSegmentation(id));
    }
    [HttpPost]
    public JsonResult GetSegment(SegmentJson json)
    {
        var segmentationLogic = new SegmentationLogic(ControllerContext, _permissionCheck);
        return Json(segmentationLogic.GetSegment(json));
    }

    [HttpPost]
    public JsonResult GetCategoriesData(int[] categoryIds)
    {
        var segmentationLogic = new SegmentationLogic(ControllerContext, _permissionCheck);
        return Json(segmentationLogic.GetCategoriesData(categoryIds));
    }

    [HttpPost]
    public JsonResult GetCategoryData(int categoryId)
    {
        var segmentationLogic = new SegmentationLogic(ControllerContext, _permissionCheck);
        return Json(segmentationLogic.GetCategoryData(categoryId));
    }

    [HttpPost]
    public JsonResult GetSegmentData(int categoryId)
    {
        var segmentationLogic = new SegmentationLogic(ControllerContext, _permissionCheck);
        return Json(segmentationLogic.GetSegmentData(categoryId));
    }
}






