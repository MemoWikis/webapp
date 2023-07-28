using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace VueApp;

public class TopicRelationEditController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly EditControllerLogic _editControllerLogic;

    public TopicRelationEditController(IGlobalSearch search,
        SessionUser sessionUser,
        PermissionCheck permissionCheck,
        CategoryRepository categoryRepository,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        EditControllerLogic editControllerLogic) 
    {
        _sessionUser = sessionUser;
        _editControllerLogic = editControllerLogic;
    }

    [HttpPost]
    public JsonResult ValidateName(string name)
    {
        var data = _editControllerLogic.ValidateName(name);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult QuickCreate(string name, int parentTopicId)
    {
        var data = _editControllerLogic            
            .QuickCreate(name, parentTopicId,_sessionUser); 

        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<JsonResult> SearchTopic(string term, int[] topicIdsToFilter = null)
    {
        var data = _editControllerLogic.SearchTopic(term, topicIdsToFilter);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<JsonResult> SearchTopicInPersonalWiki(string term, int[] topicIdsToFilter = null)
    {
        var data = _editControllerLogic.SearchTopicInPersonalWiki(term, topicIdsToFilter);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult MoveChild(int childId, int parentIdToRemove, int parentIdToAdd)
    {
        var data = _editControllerLogic.MoveChild(childId,parentIdToRemove,parentIdToAdd);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult AddChild(int childId, int parentId, int parentIdToRemove = -1, bool redirectToParent = false, bool addIdToWikiHistory = false)
    {
        var data = _editControllerLogic.AddChild(childId,parentId,parentIdToRemove,redirectToParent,addIdToWikiHistory);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult RemoveParent(int parentIdToRemove, int childId, int[] affectedParentIdsByMove = null)
    {
        var data = _editControllerLogic.RemoveParent(parentIdToRemove,childId,affectedParentIdsByMove);
        return Json(data, JsonRequestBehavior.AllowGet);
    }
}