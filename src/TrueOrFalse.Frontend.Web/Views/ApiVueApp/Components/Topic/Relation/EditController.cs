using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Utilities.ScheduledJobs;

namespace VueApp;

public class TopicRelationEditController : BaseController
{
    private readonly CategoryRepository _categoryRepository = Sl.CategoryRepo;
    private readonly IGlobalSearch _search;
    public TopicRelationEditController(IGlobalSearch search, SessionUser sessionUser) : base(sessionUser)
    {
        _search = search ?? throw new ArgumentNullException(nameof(search));
    }

    [HttpPost]
    public JsonResult ValidateName(string name)
    {
        var data = new EditControllerLogic(_search, IsInstallationAdmin).ValidateName(name);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult QuickCreate(string name, int parentTopicId)
    {
        var data = new EditControllerLogic(_search, IsInstallationAdmin)
            .QuickCreate(name, parentTopicId); 

        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<JsonResult> SearchTopic(string term, int[] topicIdsToFilter = null)
    {
        var data = new EditControllerLogic(_search, IsInstallationAdmin).SearchTopic(term, topicIdsToFilter);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<JsonResult> SearchTopicInPersonalWiki(string term, int[] topicIdsToFilter = null)
    {
        var data = new EditControllerLogic(_search, IsInstallationAdmin).SearchTopicInPersonalWiki(term, topicIdsToFilter);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult MoveChild(int childId, int parentIdToRemove, int parentIdToAdd)
    {
        var data = new EditControllerLogic(_search, IsInstallationAdmin).MoveChild(childId,parentIdToRemove,parentIdToAdd);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult AddChild(int childId, int parentId, int parentIdToRemove = -1, bool redirectToParent = false, bool addIdToWikiHistory = false)
    {
        var data = new EditControllerLogic(_search, IsInstallationAdmin).AddChild(childId,parentId,parentIdToRemove,redirectToParent,addIdToWikiHistory);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult RemoveParent(int parentIdToRemove, int childId, int[] affectedParentIdsByMove = null)
    {
        var data = new EditControllerLogic(_search, IsInstallationAdmin).RemoveParent(parentIdToRemove,childId,affectedParentIdsByMove);
        return Json(data, JsonRequestBehavior.AllowGet);
    }
}