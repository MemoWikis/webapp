public class BaseModel : BaseResolve
{
    public SponsorModel SponsorModel
    {
        get
        {
            if (_sponsorModel != null)
            {
                return _sponsorModel;
            }

            _sponsorModel = new SponsorModel();
            return _sponsorModel;
        }

        set => _sponsorModel = value;
    }

    private SponsorModel _sponsorModel;
    protected SessionUser _sessionUser => Resolve<SessionUser>();
    protected SessionUiData _sessionUiData => Resolve<SessionUiData>();

    public bool IsLoggedIn => _sessionUser.IsLoggedIn;
    public bool IsInstallationAdmin => _sessionUser.IsInstallationAdmin;

    public int UserId => _sessionUser.UserId;

    public Game UpcomingGame;

    public UserMenu UserMenu;
    
    public SidebarModel SidebarModel = new SidebarModel();

    public TopicMenu TopicMenu;

    public TopNavMenu TopNavMenu = new TopNavMenu();

    public bool ShowUserReportWidget = true;

    public BaseModel()
    {
        var sessionUiData = Resolve<SessionUiData>();
        UserMenu = sessionUiData.UserMenu;

        TopicMenu = Sl.SessionUiData.TopicMenu;
    }

    public string UserMenuActive(UserMenuEntry userMenuEntry)
    {
        return  UserMenu.UserMenuActive(userMenuEntry);
    }

    public bool UserMenuIsActive(UserMenuEntry userMenuEntry)
    {
        return !string.IsNullOrEmpty(UserMenu.UserMenuActive(userMenuEntry));
    }
}