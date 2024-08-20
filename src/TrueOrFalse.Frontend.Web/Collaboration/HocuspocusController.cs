using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class HocuspocusController() : Controller
{
    public readonly record struct AuthoriseRequest(string Token, string HocuspocusKey, int TopicId);

    [HttpPost]
    public bool Authorise([FromBody] AuthoriseRequest req)
    {
        if (req.HocuspocusKey != Settings.CollaborationHocuspocusSecretKey)
            return false;

        var (isValid, userId) = new CollaborationToken().ValidateAndGetUserId(req.Token);

        if (isValid == false)
            return false;

        var permissionCheck = new PermissionCheck(userId);
        if (permissionCheck.CanEditCategory(req.TopicId))
            return true;

        return false;
    }

    [HttpGet]
    public bool Test()
    {
        return true;
    }
}