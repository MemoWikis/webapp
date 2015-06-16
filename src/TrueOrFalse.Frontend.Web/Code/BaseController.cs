using System.Web.Mvc;

[AccessBeta]
public class BaseController : Controller
{
    protected SessionUser _sessionUser{ get { return Resolve<SessionUser>(); } }
    protected SessionUiData _sessionUiData { get { return Resolve<SessionUiData>(); } }
    public int UserId { get { return _sessionUser.UserId; } }

    /// <summary>The user fresh from the db</summary>
    public User UserFresh()
    {
        return R<UserRepo>().GetById(UserId);
    }

    protected T Resolve<T>()
    {
        return ServiceLocator.Resolve<T>();
    }

    protected T R<T>()
    {
        return ServiceLocator.Resolve<T>();
    }
}