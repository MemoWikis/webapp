using System.Collections.Generic;
using System.Linq;

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

    private SponsorModel _sponsorModel;
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

    public SidebarModel()
    {
        var userSession = Resolve<SessionUser>();
        var sessionUiData = Resolve<SessionUiData>();

        MainMenu = sessionUiData.MainMenu;

        if (userSession.User != null)
        {
            IsInstallationAdmin = userSession.IsInstallationAdmin;

            WishKnowledgeCount = Resolve<GetWishQuestionCountCached>().Run(userSession.User.Id);
            UnreadMessageCount = Resolve<GetUnreadMessageCount>().Run(userSession.User.Id);
        }
    }

    public string Active(MainMenuEntry mainMenuEntry)
    {
        return MainMenu.Active(mainMenuEntry);
    }

    public bool IsActive(MainMenuEntry mainMenuEntry)
    {
        return !string.IsNullOrEmpty(MainMenu.Active(mainMenuEntry));
    }
    
    public bool Show() => Authors.Any() || SponsorModel.IsAdFree || SuggestionCategory != null;

    public void Fill(IList<UserTinyModel> authors, int currentUserId)
    {
        foreach (var author in authors)
        {
            Authors.Add(new SidebarAuthorModel
            {
                ImageUrl = new UserImageSettings(author.Id).GetUrl_250px(author).Url,
                User = author,
                Reputation = author.Reputation,
                ReputationPos = author.ReputationPos
            });
        }

        if (authors.Count == 1)
        {
            if (!authors[0].IsKnown)
            {
                Reputation = Resolve<ReputationCalc>().Run(authors[0].User);
                AmountWishCountQuestions = Resolve<GetWishQuestionCount>().Run(authors[0].Id);
                var followerIAm = R<FollowerIAm>().Init(new List<int> { authors[0].Id }, currentUserId);
                DoIFollow = followerIAm.Of(authors[0].Id);
                IsCurrentUser = authors[0].Id == currentUserId && IsLoggedIn;
                Authors[0].ShowWishKnowledge = authors[0].ShowWishKnowledge;
            }
            else
            {
                Reputation = new ReputationCalcResult { User = authors[0] };
                Authors[0].ShowWishKnowledge = false;
                AmountWishCountQuestions = 0;
                DoIFollow = false;
                IsCurrentUser = false;
            }
        }
    }
}

public class SidebarAuthorModel
{
    public string Name => User.Name;
    public string ImageUrl;
    public UserTinyModel User;
    public bool ShowWishKnowledge;
    public int Reputation;
    public int ReputationPos;
}