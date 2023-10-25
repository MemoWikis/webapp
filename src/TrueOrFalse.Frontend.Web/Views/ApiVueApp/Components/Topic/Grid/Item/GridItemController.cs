using Microsoft.AspNetCore.Mvc;

namespace VueApp;
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
    public JsonResult GetChildren([FromRoute] int id)
    {
        var topic = EntityCache.GetCategory(id);
        if (!_permissionCheck.CanView(topic))
            return Json(new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.Category.MissingRights });


        var children = _gridItemLogic.GetChildren(id);
        return Json(new RequestResult { success = true, data = children });
    }

    [HttpGet]
    public JsonResult GetItem([FromRoute] int id)
    {
        var topic = EntityCache.GetCategory(id);
        if (!_permissionCheck.CanView(topic))
            return Json(new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.Category.MissingRights });


        var gridItem = _gridItemLogic.BuildGridTopicItem(topic);
        return Json(new RequestResult { success = true, data = gridItem });
    }
}






