using System.Web.Mvc;

public class BaseController : Controller
{
    protected SessionUiData _sessionUiData => Resolve<SessionUiData>();

    public int UserId => SessionUser.UserId;

    public bool IsLoggedIn => SessionUser.IsLoggedIn;
    public bool IsInstallationAdmin => SessionUser.IsInstallationAdmin;
    public bool IsMemuchoUser => IsLoggedIn && Settings.MemuchoUserId == UserId;
    public bool IsFacebookUser => IsLoggedIn && SessionUser.User.IsFacebookUser;

    /// <summary>The user fresh from the db</summary>
    public User User_() => R<UserRepo>().GetById(UserId);
    public User MemuchoUser() => Sl.UserRepo.GetMemuchoUser();

    protected T Resolve<T>() => ServiceLocator.Resolve<T>();

    protected T R<T>() => ServiceLocator.Resolve<T>();
}