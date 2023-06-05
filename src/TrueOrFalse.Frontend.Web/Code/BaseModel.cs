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
    protected SessionUiData _sessionUiData => Resolve<SessionUiData>();

    public bool IsLoggedIn => SessionUserLegacy.IsLoggedIn;
    public bool IsInstallationAdmin => SessionUserLegacy.IsInstallationAdmin;

    public int UserId => SessionUserLegacy.UserId;

    public UserMenu UserMenu;
    
    public SidebarModel SidebarModel = new SidebarModel();

    public TopNavMenu TopNavMenu = new TopNavMenu();

    public BaseModel()
    {
        var sessionUiData = Resolve<SessionUiData>();
        UserMenu = sessionUiData.UserMenu;
    }

    public string UserMenuActive(UserMenuEntry userMenuEntry)
    {
        return  UserMenu.UserMenuActive(userMenuEntry);
    }

    public bool ShowSidebar = false;
}