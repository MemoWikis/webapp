﻿using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace VueApp;

public class TopicRelationEditController : Controller
{
    private readonly IGlobalSearch _search;
    private readonly SessionUser _sessionUser;
    private readonly PermissionCheck _permissionCheck;
    private readonly CategoryRepository _categoryRepository;
    private readonly bool IsInstallationAdmin;

    public TopicRelationEditController(IGlobalSearch search,
        SessionUser sessionUser,
        PermissionCheck permissionCheck,
        CategoryRepository categoryRepository) 
    {
        _search = search;
        _sessionUser = sessionUser;
        _permissionCheck = permissionCheck;
        _categoryRepository = categoryRepository;
        IsInstallationAdmin = _sessionUser.IsInstallationAdmin;

    }

    [HttpPost]
    public JsonResult ValidateName(string name)
    {
        var data = new EditControllerLogic(_search, IsInstallationAdmin, _permissionCheck, _sessionUser,_categoryRepository).ValidateName(name);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult QuickCreate(string name, int parentTopicId)
    {
        var data = new EditControllerLogic(_search, IsInstallationAdmin, _permissionCheck, _sessionUser, _categoryRepository)            
            .QuickCreate(name, parentTopicId,_sessionUser); 

        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<JsonResult> SearchTopic(string term, int[] topicIdsToFilter = null)
    {
        var data = new EditControllerLogic(_search, IsInstallationAdmin, _permissionCheck, _sessionUser, _categoryRepository).SearchTopic(term, topicIdsToFilter);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<JsonResult> SearchTopicInPersonalWiki(string term, int[] topicIdsToFilter = null)
    {
        var data = new EditControllerLogic(_search, IsInstallationAdmin, _permissionCheck, _sessionUser, _categoryRepository).SearchTopicInPersonalWiki(term, topicIdsToFilter);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult MoveChild(int childId, int parentIdToRemove, int parentIdToAdd)
    {
        var data = new EditControllerLogic(_search, IsInstallationAdmin, _permissionCheck, _sessionUser, _categoryRepository).MoveChild(childId,parentIdToRemove,parentIdToAdd);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult AddChild(int childId, int parentId, int parentIdToRemove = -1, bool redirectToParent = false, bool addIdToWikiHistory = false)
    {
        var data = new EditControllerLogic(_search, IsInstallationAdmin, _permissionCheck, _sessionUser, _categoryRepository).AddChild(childId,parentId,parentIdToRemove,redirectToParent,addIdToWikiHistory);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult RemoveParent(int parentIdToRemove, int childId, int[] affectedParentIdsByMove = null)
    {
        var data = new EditControllerLogic(_search, IsInstallationAdmin, _permissionCheck, _sessionUser, _categoryRepository).RemoveParent(parentIdToRemove,childId,affectedParentIdsByMove);
        return Json(data, JsonRequestBehavior.AllowGet);
    }
}