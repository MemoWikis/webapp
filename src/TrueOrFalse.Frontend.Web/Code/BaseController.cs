using Microsoft.AspNetCore.Mvc;

public class BaseController : Controller
{
    protected SessionUser _sessionUser;

    public BaseController(SessionUser sessionUser)
    {
        _sessionUser = sessionUser;
    }
    public int UserId => _sessionUser.UserId;

    public bool IsLoggedIn => _sessionUser.IsLoggedIn;
    public bool IsInstallationAdmin => _sessionUser.IsInstallationAdmin;
    /// <summary>The user fresh from the db</summary>
}