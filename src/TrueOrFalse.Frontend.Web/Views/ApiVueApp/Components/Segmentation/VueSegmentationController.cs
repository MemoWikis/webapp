using System.Web.Mvc;

namespace VueApp;
[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class VueSegmentationController : BaseController
{
    private readonly PermissionCheck _permissionCheck;
    private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;
    private readonly CategoryValuationRepo _categoryValuationRepo;
    private readonly ImageMetaDataRepo _imageMetaDataRepo;

    public VueSegmentationController(PermissionCheck permissionCheck,
        SessionUser sessionUser,
        KnowledgeSummaryLoader knowledgeSummaryLoader,
        CategoryValuationRepo categoryValuationRepo,
        ImageMetaDataRepo imageMetaDataRepo) :base(sessionUser)
    {
        _permissionCheck = permissionCheck;
        _knowledgeSummaryLoader = knowledgeSummaryLoader;
        _categoryValuationRepo = categoryValuationRepo;
        _imageMetaDataRepo = imageMetaDataRepo;
        _sessionUser = sessionUser;
    }
    [HttpPost]
    public JsonResult GetSegmentation(int id)
    {
        var segmentationLogic = new SegmentationLogic(ControllerContext,_permissionCheck, _sessionUser,_categoryValuationRepo, _knowledgeSummaryLoader, _imageMetaDataRepo);
        return Json(segmentationLogic.GetSegmentation(id));
    }
    [HttpPost]
    public JsonResult GetSegment(SegmentJson json)
    {
        var segmentationLogic = new SegmentationLogic(ControllerContext, _permissionCheck, _sessionUser, _categoryValuationRepo, _knowledgeSummaryLoader, _imageMetaDataRepo);
        return Json(segmentationLogic.GetSegment(json));
    }

    [HttpPost]
    public JsonResult GetCategoriesData(int[] categoryIds)
    {
        var segmentationLogic = new SegmentationLogic(ControllerContext, _permissionCheck, _sessionUser, _categoryValuationRepo, _knowledgeSummaryLoader, _imageMetaDataRepo);
        return Json(segmentationLogic.GetCategoriesData(categoryIds));
    }

    [HttpPost]
    public JsonResult GetCategoryData(int categoryId)
    {
        var segmentationLogic = new SegmentationLogic(ControllerContext, _permissionCheck, _sessionUser, _categoryValuationRepo, _knowledgeSummaryLoader, _imageMetaDataRepo);
        return Json(segmentationLogic.GetCategoryData(categoryId));
    }

    [HttpPost]
    public JsonResult GetSegmentData(int categoryId)
    {
        var segmentationLogic = new SegmentationLogic(ControllerContext, _permissionCheck, _sessionUser, _categoryValuationRepo, _knowledgeSummaryLoader, _imageMetaDataRepo);
        return Json(segmentationLogic.GetSegmentData(categoryId));
    }
}






