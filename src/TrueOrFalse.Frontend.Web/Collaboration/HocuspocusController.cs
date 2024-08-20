using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class HocuspocusController() : Controller
{
    [HttpPost]
    public bool Authorise([FromBody] string token, string hocuspocusKey, int topicId)
    {
        if (hocuspocusKey != Settings.CollaborationHocuspocusSecretKey)
            return false;

        var (isValid, userId) = new CollaborationToken().ValidateAndGetUserId(token);

        if (isValid == false)
            return false;

        var permissionCheck = new PermissionCheck(userId);
        if (permissionCheck.CanEditCategory(topicId))
            return true;

        return false;
    }

    [HttpGet]
    public bool Test()
    {
        return true;
    }
}