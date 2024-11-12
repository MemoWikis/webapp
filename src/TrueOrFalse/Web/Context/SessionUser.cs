using Microsoft.AspNetCore.Http;

using TrueOrFalse.Web.Context;

public class SessionUser : IRegisterAsInstancePerLifetime
{
    private readonly HttpContext _httpContext;
    private readonly ExtendedUserCache _extendedUserCache;

    public SessionUser(
        IHttpContextAccessor httpContextAccessor,
        ExtendedUserCache extendedUserCache)
    {
        _httpContext = httpContextAccessor.HttpContext;
        _extendedUserCache = extendedUserCache;
    }

    public bool SessionIsActive() => _httpContext.Session is not null;

    public bool HasBetaAccess
    {
        get => _httpContext.Session.GetBool("isBetaLogin");
        set => _httpContext.Session.SetBool("isBetaLogin", value);
    }

    public bool IsLoggedIn
    {
        get => _httpContext.Session.GetBool("isLoggedIn");
        private set => _httpContext.Session.SetBool("isLoggedIn", value);
    }

    public bool IsInstallationAdmin
    {
        get => _httpContext.Session.GetBool("isAdministrativeLogin");
        set => _httpContext.Session.SetBool("isAdministrativeLogin", value);
    }

    public int UserId => _userId;

    private int _userId
    {
        get => _httpContext.Session.GetInt32("userId") ?? 0;
        set => _httpContext.Session.SetInt32("userId", value);
    }

    private int _last
    {
        get => _httpContext.Session.GetInt32("userId") ?? 0;
        set => _httpContext.Session.SetInt32("userId", value);
    }

    public ExtendedUserCacheItem User
    {
        get
        {
            if (_userId > 0)
                return _extendedUserCache.GetUser(_userId);

            throw new Exception("user is not logged in");
        }
    }

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
        CurrentWikiId = user.StartPageId;

        if (user.IsInstallationAdmin)
            IsInstallationAdmin = true;
        _extendedUserCache.Add(user.Id);
    }

    public void Logout()
    {
        IsLoggedIn = false;
        IsInstallationAdmin = false;
        _userId = -1;
        CurrentWikiId = 1;
    }

    public List<ActivityPoints> ActivityPoints =>
        _httpContext.Session.Get<List<ActivityPoints>>("pointActivities") ??
        new List<ActivityPoints>();

    public void AddPointActivity(ActivityPoints activityPoints) =>
        ActivityPoints.Add(activityPoints);


    public int GetTotalActivityPoints() =>
        ActivityPoints.Sum(activity => activity.Amount);

    public bool IsLoggedInUserOrAdmin() =>
        IsLoggedInUser(UserId) || IsInstallationAdmin;

    public int CurrentWikiId
    {
        get => _httpContext.Session.GetInt32("currentWikiId") ?? 1;
        private set => _httpContext.Session.SetInt32("currentWikiId", value);
    }

    public void SetWikiId(PageCacheItem page) => CurrentWikiId = page.Id;

    public void SetWikiId(int id) => CurrentWikiId = id;
}