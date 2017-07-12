using System;
using System.Web;

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
    
    public bool IsInGame;

    public bool IsCreatorOfGame;

    public MenuLeftModel MenuLeftModel = new MenuLeftModel();

    public bool IsThemeNavigationPage;

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
        SetLeftMenuData();
    }

    private void SetLeftMenuData()
    {
        var httpContextData = HttpContext.Current.Request.RequestContext.RouteData.Values;
        var isMainPageRequest = (string)httpContextData["controller"] == "Welcome" &&
                                (string)httpContextData["action"] == "Welcome";
        var isCategoryPageRequest = (string)httpContextData["controller"] == "Category" &&
                                 (string)httpContextData["action"] == "Category";
        var isCategoriesPageRequest = (string)httpContextData["controller"] == "Categories" &&
                                    (string)httpContextData["action"] == "Categories";
        var isEditCategoryPageRequest = (string)httpContextData["controller"] == "EditCategory" &&
                                        (string)httpContextData["action"] == "Edit";
        var isSetPageRequest = (string)httpContextData["controller"] == "EditSet" &&
                                        (string)httpContextData["action"] == "QuestionSet";
        var isSetsPageRequest = (string)httpContextData["controller"] == "Sets" &&
                                        (string)httpContextData["action"] == "Sets";
        var isEditSetsPageRequest = (string)httpContextData["controller"] == "EditSet" &&
                                        (string)httpContextData["action"] == "Edit";

        if (isCategoryPageRequest || isMainPageRequest || isCategoriesPageRequest || isEditCategoryPageRequest || isSetPageRequest || isSetsPageRequest || isEditSetsPageRequest)
            IsThemeNavigationPage = true;

        if (isCategoryPageRequest || isEditCategoryPageRequest)
            MenuLeftModel.ActualCategory = Sl.CategoryRepo.GetById(Convert.ToInt32(httpContextData["id"]));
    }
}