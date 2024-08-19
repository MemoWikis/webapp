using Microsoft.AspNetCore.Mvc;

namespace Collaboration;

public class HocuspocusController() : Controller
{
    [HttpGet]
    public bool Authorise(string token, string hocuspocusKey, int topicId)
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
}