using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class HocuspocusController : Controller
{
    public readonly record struct AuthoriseRequest(string Token, string HocuspocusKey, int TopicId);

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
        if (!permissionCheck.CanEditCategory(req.TopicId))
        {
            Logg.r.Error("Collaboration - Authorise: No Permission - userId:{0}, topicId:{1}", userId, req.TopicId);
            return false;
        }

        return true;
    }

}