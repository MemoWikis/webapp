using System.Web.Mvc;

[AccessBeta]
public class BaseController : Controller
{
    protected SessionUser _sessionUser => Resolve<SessionUser>();
    protected SessionUiData _sessionUiData => Resolve<SessionUiData>();
    public int UserId => _sessionUser.UserId;

    /// <summary>The user fresh from the db</summary>
    public User UserFresh(){ return R<UserRepo>().GetById(UserId);}
    public User MemuchoUser(){ return R<UserRepo>().GetById(Settings.MemuchoUserId); }

    public bool IsMemuchoUser => _sessionUser.IsLoggedIn && Settings.MemuchoUserId == UserId;

    protected T Resolve<T>()
    {
        return ServiceLocator.Resolve<T>();
    }

    protected T R<T>()
    {
        return ServiceLocator.Resolve<T>();
    }
}