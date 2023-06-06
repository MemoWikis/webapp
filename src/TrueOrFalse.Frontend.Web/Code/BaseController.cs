using System.Web;
using System.Web.Mvc;

public class BaseController : Controller
{
    protected SessionUser _sessionUser;
    public BaseController(SessionUser sessionUser)
    {
        _sessionUser = sessionUser; 
    }
    protected SessionUiData _sessionUiData => Resolve<SessionUiData>();

    public int UserId => _sessionUser.UserId;

    public bool IsLoggedIn => _sessionUser.IsLoggedIn;
    public bool IsInstallationAdmin => _sessionUser.IsInstallationAdmin;
    /// <summary>The user fresh from the db</summary>
    public User User_() => R<UserRepo>().GetById(UserId);
    public User MemuchoUser() => Sl.UserRepo.GetMemuchoUser();

    protected T Resolve<T>() => ServiceLocator.Resolve<T>();

    protected T R<T>() => ServiceLocator.Resolve<T>();
}