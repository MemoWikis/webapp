using System.Web.Mvc;

namespace VueApp;
[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class GridController : BaseController
{
    private readonly PermissionCheck _permissionCheck;
    private readonly GridItemLogic _gridItemLogic;

    public GridController(PermissionCheck permissionCheck,SessionUser sessionUser, GridItemLogic gridItemLogic) :base(sessionUser)
    {
        _permissionCheck = permissionCheck;
        _gridItemLogic = gridItemLogic;
        _sessionUser = sessionUser;
    }

    [HttpGet]
    public JsonResult GetItem(int id)
    {
        var topic = EntityCache.GetCategory(id);
        if (!_permissionCheck.CanView(topic))
            return Json(new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.Category.MissingRights }, JsonRequestBehavior.AllowGet);


        var gridItem = _gridItemLogic.BuildGridTopicItem(topic);
        return Json(new RequestResult { success = true, data = gridItem }, JsonRequestBehavior.AllowGet);
    }
}






