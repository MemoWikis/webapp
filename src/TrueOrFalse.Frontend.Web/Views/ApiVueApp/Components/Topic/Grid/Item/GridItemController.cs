using System.Web.Mvc;

namespace VueApp;
[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class GridItemController : BaseController
{
    private readonly PermissionCheck _permissionCheck;
    private readonly GridItemLogic _gridItemLogic;

    public GridItemController(PermissionCheck permissionCheck,SessionUser sessionUser, GridItemLogic gridItemLogic) :base(sessionUser)
    {
        _permissionCheck = permissionCheck;
        _gridItemLogic = gridItemLogic;
        _sessionUser = sessionUser;
    }

    [HttpGet]
    public JsonResult GetChildren(int id)
    {
        var topic = EntityCache.GetCategory(id);
        if (!_permissionCheck.CanView(topic))
            return Json(new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.Category.MissingRights }, JsonRequestBehavior.AllowGet);


        var children = _gridItemLogic.GetChildren(id);
        return Json(new RequestResult { success = true, data = children }, JsonRequestBehavior.AllowGet);
    }
}






