using System;
using System.Collections.Generic;

public class SidebarModel : BaseResolve
{
    public int WishKnowledgeCount;
    public bool IsInstallationAdmin;
    public MainMenu MainMenu;

    protected SessionUser _sessionUser => Resolve<SessionUser>();
    public bool IsLoggedIn => _sessionUser.IsLoggedIn;

    public int UnreadMessageCount = 0;

    

    public bool IsWelcomePage;

    public CategoryNavigationModel categoryNavigationModel;

    public IList<SidebarAuthorModel> Authors = new List<SidebarAuthorModel>();

    public ReputationCalcResult Reputation;
    public int AmountWishCountQuestions;
    public bool DoIFollow;
    public bool IsCurrentUser;
    public string AuthorCardLinkText;
    public string AuthorImageUrl;

    public string CategorySuggestionImageUrl;
    public string CategorySuggestionUrl;
    public Category SuggestionCategory;

    public SidebarModel()
    {
        var userSession = Resolve<SessionUser>();
        var sessionUiData= Resolve<SessionUiData>();

        MainMenu = sessionUiData.MainMenu;

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

    public string Active(MainMenuEntry mainMenuEntry)
    {
        return MainMenu.Active(mainMenuEntry);
    }

    public bool IsActive(MainMenuEntry mainMenuEntry)
    {
        return !string.IsNullOrEmpty(MainMenu.Active(mainMenuEntry));
    }
}

public class SidebarAuthorModel
{
    public string Name => User.Name;
    public string ImageUrl;
    public User User;
    public Boolean ShowWishKnowledge;
}