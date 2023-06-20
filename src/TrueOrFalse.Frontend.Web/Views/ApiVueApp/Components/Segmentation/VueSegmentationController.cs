using System.Web.Mvc;

namespace VueApp;
[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class VueSegmentationController : BaseController
{
    private readonly PermissionCheck _permissionCheck;

    public VueSegmentationController(PermissionCheck permissionCheck,SessionUser sessionUser) :base(sessionUser)
    {
        _permissionCheck = permissionCheck;
        _sessionUser = sessionUser;
    }
    [HttpPost]
    public JsonResult GetSegmentation(int id)
    {
        var segmentationLogic = new SegmentationLogic(ControllerContext,_permissionCheck, _sessionUser);
        return Json(segmentationLogic.GetSegmentation(id));
    }
    [HttpPost]
    public JsonResult GetSegment(SegmentJson json)
    {
        var segmentationLogic = new SegmentationLogic(ControllerContext, _permissionCheck,_sessionUser);
        return Json(segmentationLogic.GetSegment(json));
    }

    [HttpPost]
    public JsonResult GetCategoriesData(int[] categoryIds)
    {
        var segmentationLogic = new SegmentationLogic(ControllerContext, _permissionCheck, _sessionUser);
        return Json(segmentationLogic.GetCategoriesData(categoryIds));
    }

    [HttpPost]
    public JsonResult GetCategoryData(int categoryId)
    {
        var segmentationLogic = new SegmentationLogic(ControllerContext, _permissionCheck, _sessionUser);
        return Json(segmentationLogic.GetCategoryData(categoryId));
    }

    [HttpPost]
    public JsonResult GetSegmentData(int categoryId)
    {
        var segmentationLogic = new SegmentationLogic(ControllerContext, _permissionCheck, _sessionUser);
        return Json(segmentationLogic.GetSegmentData(categoryId));
    }
}






