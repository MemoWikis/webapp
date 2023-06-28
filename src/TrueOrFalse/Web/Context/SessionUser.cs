using Seedworks.Web.State;
using System.Collections.Generic;
using System.Web.Security;
using System.Web;

public class SessionUser :IRegisterAsInstancePerLifetime
{
    private readonly HttpContext _httpContext;
    private readonly CategoryValuationRepo _categoryValuationRepo;

    public SessionUser(HttpContext httpContext, CategoryValuationRepo categoryValuationRepo)
    {
        _httpContext = httpContext;
        _categoryValuationRepo = categoryValuationRepo;
    }

    public bool IsSesionActive () => _httpContext.Session is not null;

    public bool HasBetaAccess
    {
        get => _httpContext.Session["isBetaLogin"] as bool? ?? false;
        set => _httpContext.Session.Add("isBetaLogin", value);
    }

    public bool IsLoggedIn
    {
        get => _httpContext.Session["isLoggedIn"] as bool? ?? false;
        private set => _httpContext.Session.Add("isLoggedIn", value);
    }

    public bool IsInstallationAdmin
    {
        get => _httpContext.Session["isAdministrativeLogin"] as bool? ?? false;
        set => _httpContext.Session.Add("isAdministrativeLogin", value);
    }

    public int UserId => _userId;

    private int _userId
    {
        get => _httpContext.Session["userId"] as int? ?? 0;
        set => _httpContext.Session.Add("userId", value);
    }

    public SessionUserCacheItem User => _userId < 0 ? null : SessionUserCache.GetUser(_userId, _categoryValuationRepo);

    public bool IsLoggedInUser(int userId)
    {
        if (!IsLoggedIn)
            return false;

        return userId == UserId;
    }

    public void Login(User user)
    {
        HasBetaAccess = true;
        IsLoggedIn = true;
        _userId = user.Id;
        CurrentWikiId = user.StartTopicId;

        if (user.IsInstallationAdmin)
            IsInstallationAdmin = true;

        if (_httpContext != null)
            FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);

        SessionUserCache.CreateItemFromDatabase(user.Id, _categoryValuationRepo);
    }

    public void Logout()
    {
        IsLoggedIn = false;
        IsInstallationAdmin = false;
        _userId = -1;
        CurrentWikiId = 1;

        if (HttpContext.Current != null)
            FormsAuthentication.SignOut();
    }


    public List<ActivityPoints> ActivityPoints => _httpContext.Session["pointActivities"] as List<ActivityPoints> ?? new List<ActivityPoints>();

    public void AddPointActivity(ActivityPoints activityPoints)
    {
        ActivityPoints.Add(activityPoints);
    }

    public int GetTotalActivityPoints()
    {
        int totalPoints = 0;

        foreach (var activity in ActivityPoints)
            totalPoints += activity.Amount;

        return totalPoints;
    }
    public bool IsLoggedInUserOrAdmin()
    {
        return IsLoggedInUser(UserId) || IsInstallationAdmin;
    }
    public int CurrentWikiId
    {
        get => _httpContext.Session["currentWikiId"] as int? ?? 1;
        private set => _httpContext.Session.Add("currentWikiId", value);
    }

    public  void SetWikiId(CategoryCacheItem category) => CurrentWikiId = category.Id;
    public  void SetWikiId(int id) => CurrentWikiId = id;
}