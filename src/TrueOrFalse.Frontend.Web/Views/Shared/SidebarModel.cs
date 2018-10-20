using System.Collections.Generic;

public class SidebarModel : BaseResolve
{
    public int WishKnowledgeCount;
    public bool IsInstallationAdmin;
    public Menu Menu;

    protected SessionUser _sessionUser => Resolve<SessionUser>();
    public bool IsLoggedIn => _sessionUser.IsLoggedIn;

    public int UnreadMessageCount = 0;

    public string AutorCardLinkText;
    public string AutorImageUrl;
    public string CreatorName;
    public User Creator;

    public ReputationCalcResult Reputation;
    public int AmountWishCountQuestions;
    public bool DoIFollow;
    public bool IsCurrentUser;

    public bool IsWelcomePage;

    public CategoryNavigationModel categoryNavigationModel;

    public List<string> AuthorNames = new List<string>();
    public List<string> AuthorImageUrls = new List<string>();
    public IList<User> Authors = new List<User>();

    public string CategorySuggestionImageUrl;
    public string CategorySuggestionName;
    public string CategorySuggestionUrl;

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

    public string Active(MenuEntry menuEntry)
    {
        return Menu.Active(menuEntry);
    }

    public bool IsActive(MenuEntry menuEntry)
    {
        return !string.IsNullOrEmpty(Menu.Active(menuEntry));
    }
}