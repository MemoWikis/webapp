public class MiddlewareAuthController(SessionUser _sessionUser) : ApiBaseController
{
    [AccessOnlyAsAdmin]
    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public bool Get()
    {
        return _sessionUser.IsInstallationAdmin;
    }
}