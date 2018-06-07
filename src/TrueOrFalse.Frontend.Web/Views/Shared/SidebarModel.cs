public class SidebarModel : BaseResolve 
{
    public int WishKnowledgeCount;
    public bool IsInstallationAdmin;
    public Menu Menu;

    protected SessionUser _sessionUser => Resolve<SessionUser>();
    public bool IsLoggedIn => _sessionUser.IsLoggedIn;

    public bool HasKnowledgeBtnText = false;
    public string KnowldegeBtnText;

    public int UnreadMessageCount = 0;

    public string CardFooterText;
    public string AutorImageUrl;

    public SidebarModel()
    {
        var userSession = Resolve<SessionUser>();
        var sessionUiData= Resolve<SessionUiData>();

        Menu = sessionUiData.Menu;

        if (userSession.User != null)
        {
            IsInstallationAdmin = userSession.IsInstallationAdmin;

            WishKnowledgeCount = Resolve<GetWishQuestionCountCached>().Run(userSession.User.Id);
            UnreadMessageCount = Resolve<GetUnreadMessageCount>().Run(userSession.User.Id);
        }
    }

    public string Active(MenuEntry menuEntry)
    {
        return Menu.Active(menuEntry);
    }

    public bool IsActive(MenuEntry menuEntry)
    {
        return !string.IsNullOrEmpty(Menu.Active(menuEntry));
    }
}