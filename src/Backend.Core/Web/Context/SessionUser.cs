using Microsoft.AspNetCore.Http;

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

    public void Login(User user, PageViewRepo _pageViewRepo)
    {
        HasBetaAccess = true;
        IsLoggedIn = true;
        _userId = user.Id;
        CurrentWikiId = user.StartPageId;

        if (user.IsInstallationAdmin)
            IsInstallationAdmin = true;
        _extendedUserCache.Add(user.Id, _pageViewRepo);

        var userCacheItem = EntityCache.GetUserByIdNullable(user.Id);
        if (userCacheItem != null)
        {
            userCacheItem.SetTempShareTokens(ShareTokens);
            EntityCache.AddOrUpdate(userCacheItem);
        }
    }

    public void Logout()
    {
        IsLoggedIn = false;
        IsInstallationAdmin = false;
        _userId = -1;
        CurrentWikiId = 1;
        ClearShareTokens();
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

    public Dictionary<int, string> ShareTokens
    {
        get
        {
            if (IsLoggedIn)
            {
                var tempShareTokens = EntityCache.GetUserByIdNullable(UserId)?.TempShareTokens;
                if (tempShareTokens != null)
                    return tempShareTokens;
            }

            return _httpContext.Session.Get<Dictionary<int, string>>("shareTokens") ?? new Dictionary<int, string>();
        }
        private set => _httpContext.Session.Set("shareTokens", value);
    }

    public void AddShareToken(int pageId, string token)
    {
        var share = EntityCache.GetPageShares(pageId).FirstOrDefault(share => share.Token == token);
        if (share != null)
        {
            var tokens = ShareTokens;
            tokens[pageId] = token;
            ShareTokens = tokens;

            if (IsLoggedIn)
            {
                var userCacheItem = EntityCache.GetUserByIdNullable(UserId);
                if (userCacheItem != null)
                {
                    userCacheItem.AddTempShareToken(pageId, token);
                    EntityCache.AddOrUpdate(userCacheItem);
                }
            }
        }
        else
        {
            RemoveShareToken(pageId);
        }

    }

    public void RemoveShareToken(int pageId)
    {
        if (ShareTokens.ContainsKey(pageId))
            ShareTokens.Remove(pageId);

        if (IsLoggedIn)
        {
            var userCacheItem = EntityCache.GetUserByIdNullable(UserId);
            if (userCacheItem != null)
            {
                userCacheItem.RemoveTempShareToken(pageId);
                EntityCache.AddOrUpdate(userCacheItem);
            }
        }
    }
    public void ClearShareTokens()
    {
        ShareTokens = new Dictionary<int, string>();
        if (IsLoggedIn && _userId > 0)
        {
            // Clear TempShareTokens in UserCacheItem
            var userCacheItem = EntityCache.GetUserByIdNullable(_userId);
            if (userCacheItem != null)
            {
                userCacheItem.ClearTempShareTokens();
                EntityCache.AddOrUpdate(userCacheItem);
            }
        }
    }
}