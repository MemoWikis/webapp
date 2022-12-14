using System.Collections.Generic;
using System.Linq;

public class SidebarModel : UserCardBaseModel
{
    public int WishKnowledgeCount;
    public bool IsInstallationAdmin;
    public MainMenu MainMenu;
    public int UnreadMessageCount = 0;
    public bool IsWelcomePage;
    public IList<AuthorViewModel> Authors = new List<AuthorViewModel>();
    public string AuthorCardLinkText;
    public string AuthorImageUrl;
    public string CategorySuggestionImageUrl;
    public string CategorySuggestionUrl;
    public CategoryCacheItem SuggestionCategory;
    private SponsorModel _sponsorModel;
    public int FollowerCount;

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
        var sessionUiData = Resolve<SessionUiData>();

        MainMenu = sessionUiData.MainMenu;

        if (SessionUser.User != null)
        {
            IsInstallationAdmin = SessionUser.IsInstallationAdmin;

            WishKnowledgeCount = Resolve<GetWishQuestionCountCached>().Run(SessionUser.UserId);
            UnreadMessageCount = Resolve<GetUnreadMessageCount>().Run(SessionUser.UserId);
        }

        var a = Authors.Any().ToString();
    }

    public string Active(MainMenuEntry mainMenuEntry)
    {
        return MainMenu.Active(mainMenuEntry);
    }

    public bool IsActive(MainMenuEntry mainMenuEntry)
    {
        return !string.IsNullOrEmpty(MainMenu.Active(mainMenuEntry));

    }

    public bool Show() => Authors.Any() || SponsorModel.IsAdFree;
    public void Fill(IList<UserTinyModel> authors, int currentUserId)
    {
        if (!authors.Any()) return;
        Authors = AuthorViewModel.Convert(authors);
        FillUserCardBaseModel(authors, currentUserId);
    }
}
