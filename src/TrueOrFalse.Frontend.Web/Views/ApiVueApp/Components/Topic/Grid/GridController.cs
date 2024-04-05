using Microsoft.AspNetCore.Mvc;

namespace VueApp;
public class GridController(PermissionCheck _permissionCheck, SessionUser _sessionUser, TopicGridManager _gridItemLogic) : BaseController(_sessionUser)
{
    [HttpGet]
    public JsonResult GetItem([FromRoute] int id)
    {
        var topic = EntityCache.GetCategory(id);
        if (topic == null)
            return Json(new RequestResult { Success = false, MessageKey = FrontendMessageKeys.Error.Default });
        if (!_permissionCheck.CanView(topic))
            return Json(new RequestResult { Success = false, MessageKey = FrontendMessageKeys.Error.Category.MissingRights });

        var gridItem = _gridItemLogic.BuildGridTopicItem(topic);
        return Json(new RequestResult { Success = true, Data = gridItem });
    }
}






