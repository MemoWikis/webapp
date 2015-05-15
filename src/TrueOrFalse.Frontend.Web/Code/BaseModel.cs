public class BaseModel : BaseResolve
{
    public MenuLeftModel MenuLeftModel = new MenuLeftModel();

    protected SessionUser _sessionUser { get { return Resolve<SessionUser>(); } }
    protected SessionUiData _sessionUiData { get { return Resolve<SessionUiData>(); } }

    public bool IsLoggedIn{ get { return _sessionUser.IsLoggedIn; } }

    public int UserId{ get { return _sessionUser.UserId; } }

    public BaseModel()
    {
        
    }
}