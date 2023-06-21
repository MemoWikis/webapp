using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace VueApp;

public class TopicRelationEditController : BaseController
{
    private readonly IGlobalSearch _search;
    private readonly PermissionCheck _permissionCheck;

    public TopicRelationEditController(IGlobalSearch search, SessionUser sessionUser,PermissionCheck permissionCheck) : base(sessionUser)
    {
        _search = search ?? throw new ArgumentNullException(nameof(search));
        _permissionCheck = permissionCheck;
    }

    [HttpPost]
    public JsonResult ValidateName(string name)
    {
        var data = new EditControllerLogic(_search, IsInstallationAdmin, _permissionCheck, UserId).ValidateName(name);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult QuickCreate(string name, int parentTopicId)
    {
        var data = new EditControllerLogic(_search, IsInstallationAdmin, _permissionCheck, UserId)            
            .QuickCreate(name, parentTopicId); 

        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<JsonResult> SearchTopic(string term, int[] topicIdsToFilter = null)
    {
        var data = new EditControllerLogic(_search, IsInstallationAdmin, _permissionCheck, UserId).SearchTopic(term, topicIdsToFilter);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<JsonResult> SearchTopicInPersonalWiki(string term, int[] topicIdsToFilter = null)
    {
        var data = new EditControllerLogic(_search, IsInstallationAdmin, _permissionCheck, UserId).SearchTopicInPersonalWiki(term, topicIdsToFilter);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult MoveChild(int childId, int parentIdToRemove, int parentIdToAdd)
    {
        var data = new EditControllerLogic(_search, IsInstallationAdmin, _permissionCheck, UserId).MoveChild(childId,parentIdToRemove,parentIdToAdd);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult AddChild(int childId, int parentId, int parentIdToRemove = -1, bool redirectToParent = false, bool addIdToWikiHistory = false)
    {
        var data = new EditControllerLogic(_search, IsInstallationAdmin, _permissionCheck, UserId).AddChild(childId,parentId,parentIdToRemove,redirectToParent,addIdToWikiHistory);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult RemoveParent(int parentIdToRemove, int childId, int[] affectedParentIdsByMove = null)
    {
        var data = new EditControllerLogic(_search, IsInstallationAdmin, _permissionCheck, UserId).RemoveParent(parentIdToRemove,childId,affectedParentIdsByMove);
        return Json(data, JsonRequestBehavior.AllowGet);
    }
}