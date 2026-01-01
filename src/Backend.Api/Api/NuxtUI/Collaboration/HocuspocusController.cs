using JetBrains.Annotations;

public class HocuspocusController(SessionUser _sessionUser) : ApiBaseController
{
    public readonly record struct AuthorizeRequest(string Token, string HocuspocusKey, int PageId, [CanBeNull] string ShareToken);

    public readonly record struct AuthorizeResponse(bool CanView = false, bool CanEdit = false);

    public readonly record struct GetContentRequest(string HocuspocusKey, int PageId, string Token, [CanBeNull] string ShareToken);

    public readonly record struct GetContentResponse(bool Success, string Content);

    [HttpPost]
    public AuthorizeResponse Authorize([FromBody] AuthorizeRequest request)
    {
        if (request.HocuspocusKey != Settings.CollaborationHocuspocusSecretKey)
        {
            Log.Error("Collaboration - Authorize: Incorrect Hocuspocuskey:{0}", request.HocuspocusKey);
            return new AuthorizeResponse();
        }

        var (isValid, userId) = new CollaborationToken().ValidateAndGetUserId(request.Token);

        if (!isValid)
        {
            Log.Error("Collaboration - Authorize: Invalid Token {0}", request.Token);
            return new AuthorizeResponse();
        }

        if (request.ShareToken != null)
            _sessionUser.AddShareToken(request.PageId, request.ShareToken);

        var permissionCheck = new PermissionCheck(_sessionUser);
        if (permissionCheck.CanEditPage(request.PageId, request.ShareToken, isLoggedIn: isValid))
        {
            Log.Error("Collaboration - Authorize: No Permission - userId:{0}, pageId:{1}", userId, request.PageId);
            return new AuthorizeResponse(CanView: true, CanEdit: true);
        }

        return new AuthorizeResponse(CanView: true, CanEdit: false);
    }

    [HttpPost]
    public GetContentResponse GetContent([FromBody] GetContentRequest request)
    {
        if (request.HocuspocusKey != Settings.CollaborationHocuspocusSecretKey)
        {
            Log.Error("Collaboration - GetContent: Incorrect Hocuspocuskey:{0}", request.HocuspocusKey);
            return new GetContentResponse(Success: false, Content: "");
        }

        var (isValid, userId) = new CollaborationToken().ValidateAndGetUserId(request.Token);

        if (!isValid)
        {
            Log.Error("Collaboration - GetContent: Invalid Token {0}", request.Token);
            return new GetContentResponse(Success: false, Content: "");
        }

        if (request.ShareToken != null)
            _sessionUser.AddShareToken(request.PageId, request.ShareToken);

        var permissionCheck = new PermissionCheck(_sessionUser);
        var page = EntityCache.GetPage(request.PageId);

        if (!permissionCheck.CanView(page, request.ShareToken))
        {
            Log.Warning("Collaboration - GetContent: No Permission - userId:{0}, pageId:{1}", userId, request.PageId);
            return new GetContentResponse(Success: false, Content: "");
        }

        if (page == null)
        {
            Log.Warning("Collaboration - GetContent: Page not found - pageId:{0}", request.PageId);
            return new GetContentResponse(Success: false, Content: "");
        }

        return new GetContentResponse(Success: true, Content: page.Content ?? "");
    }

}