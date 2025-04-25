using JetBrains.Annotations;

public class HocuspocusController : ApiBaseController
{
    public readonly record struct AuthorizeRequest(string Token, string HocuspocusKey, int PageId, [CanBeNull] string ShareToken);

    public readonly record struct AuthorizeResponse(bool CanView = false, bool CanEdit = false);

    [HttpPost]
    public AuthorizeResponse Authorize([FromBody] AuthorizeRequest request)
    {
        if (request.HocuspocusKey != Settings.CollaborationHocuspocusSecretKey)
        {
            Logg.r.Error("Collaboration - Authorize: Incorrect Hocuspocuskey:{0}", request.HocuspocusKey);
            return new AuthorizeResponse();
        }

        var (isValid, userId) = new CollaborationToken().ValidateAndGetUserId(request.Token);

        if (isValid == false)
        {
            Logg.r.Error("Collaboration - Authorize: Invalid Token {0}", request.Token);
            return new AuthorizeResponse();
        }

        var permissionCheck = new PermissionCheck(userId);
        if (permissionCheck.CanEditPage(request.PageId, request.ShareToken, isLoggedIn: isValid))
        {
            Logg.r.Error("Collaboration - Authorize: No Permission - userId:{0}, pageId:{1}", userId, request.PageId);
            return new AuthorizeResponse(CanView: true, CanEdit: true);
        }

        return new AuthorizeResponse(CanView: true, CanEdit: false);
    }

}