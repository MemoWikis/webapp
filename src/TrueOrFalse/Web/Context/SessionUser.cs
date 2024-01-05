
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using TrueOrFalse.Web.Context;

public class SessionUser : IRegisterAsInstancePerLifetime
{
    private readonly HttpContext _httpContext;
    private readonly SessionUserCache _sessionUserCache;

    public SessionUser(IHttpContextAccessor httpContextAccessor,
        SessionUserCache sessionUserCache)
    {
        _httpContext = httpContextAccessor.HttpContext; ;
        _sessionUserCache = sessionUserCache;
    }

    public bool SessionIsActive () => _httpContext.Session is not null;

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

    public SessionUserCacheItem User => _userId < 0 ? null : _sessionUserCache.GetUser(_userId);
    //public SessionUserCacheItem User => _userId < 0 ? null : GetOrCreateUserFromSessionCache();

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

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        var userCacheItem = _sessionUserCache.CreateSessionUserItemFromDatabase(user.Id);
        _sessionUserCache.AddOrUpdate(userCacheItem);
    }

    public async void Logout()
    {
        IsLoggedIn = false;
        IsInstallationAdmin = false;
        _userId = -1;
        CurrentWikiId = 1;

        if (_httpContext != null)
           await _httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }


    public List<ActivityPoints> ActivityPoints => _httpContext.Session.Get<List<ActivityPoints>>("pointActivities") ?? new List<ActivityPoints>();

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
        get => _httpContext.Session.GetInt32("currentWikiId") ?? 1;
        private set => _httpContext.Session.SetInt32("currentWikiId", value);
    }

    public void SetWikiId(CategoryCacheItem category) => CurrentWikiId = category.Id;
    public void SetWikiId(int id) => CurrentWikiId = id;

   
}