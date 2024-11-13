using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class HocuspocusController : Controller
{
    public readonly record struct AuthoriseRequest(string Token, string HocuspocusKey, int PageId);

    [HttpPost]
    public bool Authorise([FromBody] AuthoriseRequest req)
    {
        if (req.HocuspocusKey != Settings.CollaborationHocuspocusSecretKey)
        {
            Logg.r.Error("Collaboration - Authorise: Incorrect Hocuspocuskey:{0}", req.HocuspocusKey);
            return false;
        }

        var (isValid, userId) = new CollaborationToken().ValidateAndGetUserId(req.Token);

        if (isValid == false)
        {
            Logg.r.Error("Collaboration - Authorise: Invalid Token {0}", req.Token);
            return false;
        }

        var permissionCheck = new PermissionCheck(userId);
        if (!permissionCheck.CanEditPage(req.PageId))
        {
            Logg.r.Error("Collaboration - Authorise: No Permission - userId:{0}, pageId:{1}", userId, req.PageId);
            return false;
        }

        return true;
    }

}