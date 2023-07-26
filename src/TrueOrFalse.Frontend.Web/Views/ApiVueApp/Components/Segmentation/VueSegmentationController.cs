using System.Web.Mvc;

namespace VueApp;
[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class VueSegmentationController : BaseController
{
    private readonly PermissionCheck _permissionCheck;
    private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;
    private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
    private readonly ImageMetaDataRepo _imageMetaDataRepo;
    private readonly SegmentationLogic _segmentationLogic;

    public VueSegmentationController(PermissionCheck permissionCheck,
        SessionUser sessionUser,
        KnowledgeSummaryLoader knowledgeSummaryLoader,
        CategoryValuationReadingRepo categoryValuationReadingRepo,
        ImageMetaDataRepo imageMetaDataRepo,
        SegmentationLogic segmentationLogic) :base(sessionUser)
    {
        _permissionCheck = permissionCheck;
        _knowledgeSummaryLoader = knowledgeSummaryLoader;
        _categoryValuationReadingRepo = categoryValuationReadingRepo;
        _imageMetaDataRepo = imageMetaDataRepo;
        _segmentationLogic = segmentationLogic;
        _sessionUser = sessionUser;
    }
    [HttpPost]
    public JsonResult GetSegmentation(int id)
    {
        return Json(_segmentationLogic.GetSegmentation(id));
    }
    [HttpPost]
    public JsonResult GetSegment(SegmentJson json)
    {
        return Json(_segmentationLogic.GetSegment(json));
    }

    [HttpPost]
    public JsonResult GetCategoriesData(int[] categoryIds)
    {
        return Json(_segmentationLogic.GetCategoriesData(categoryIds));
    }

    [HttpPost]
    public JsonResult GetCategoryData(int categoryId)
    {
        return Json(_segmentationLogic.GetCategoryData(categoryId));
    }

    [HttpPost]
    public JsonResult GetSegmentData(int categoryId)
    {
        return Json(_segmentationLogic.GetSegmentData(categoryId));
    }
}






