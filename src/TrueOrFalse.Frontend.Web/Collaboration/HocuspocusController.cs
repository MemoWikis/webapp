using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class HocuspocusController : Controller
{
    public readonly record struct AuthorizeRequest(string Token, string HocuspocusKey, int PageId, [CanBeNull] string ShareToken);

    public readonly record struct AuthorizeResponse(bool CanView = false, bool CanEdit = false);

    [HttpPost]
    public AuthorizeResponse Authorize([FromBody] AuthorizeRequest req)
    {
        if (req.HocuspocusKey != Settings.CollaborationHocuspocusSecretKey)
        {
            Logg.r.Error("Collaboration - Authorize: Incorrect Hocuspocuskey:{0}", req.HocuspocusKey);
            return new AuthorizeResponse();
        }

        var (isValid, userId) = new CollaborationToken().ValidateAndGetUserId(req.Token);

        if (isValid == false)
        {
            Logg.r.Error("Collaboration - Authorize: Invalid Token {0}", req.Token);
            return new AuthorizeResponse();
        }

        var permissionCheck = new PermissionCheck(userId);
        if (permissionCheck.CanEditPage(req.PageId, req.ShareToken, isLoggedIn: isValid))
        {
            Logg.r.Error("Collaboration - Authorize: No Permission - userId:{0}, pageId:{1}", userId, req.PageId);
            return new AuthorizeResponse(CanView: true, CanEdit: true);
        }

        return new AuthorizeResponse(CanView: true, CanEdit: false);
    }

}