public class BaseModel : BaseResolve
{
    protected SessionUser _sessionUser => Resolve<SessionUser>();
    protected SessionUiData _sessionUiData => Resolve<SessionUiData>();

    public bool IsLoggedIn => _sessionUser.IsLoggedIn;
    public bool IsInstallationAdmin => _sessionUser.IsInstallationAdmin;

    public int UserId => _sessionUser.UserId;

    public Game UpcomingGame;
    
    public bool IsInGame;

    public bool IsCreatorOfGame;

    public SidebarModel SidebarModel = new SidebarModel();

    public TopicMenu TopicMenu;

    public TopNavMenu TopNavMenu = new TopNavMenu();

    public bool ShowUserReportWidget = true;

    public BaseModel()
    {
        if (IsLoggedIn)
        {
            var isInGame = R<IsCurrentlyIn>().Game(UserId);
            if (isInGame.Yes)
            {
                UpcomingGame = isInGame.Game;
                IsInGame = true;
                IsCreatorOfGame = isInGame.IsCreator;
            }
            else
                UpcomingGame = new Game();
        }

        TopicMenu = Sl.SessionUiData.TopicMenu;
    }
}