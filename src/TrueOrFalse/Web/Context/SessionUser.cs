using Seedworks.Web.State;
using System.Collections.Generic;
using System.Web.Security;
using System.Web;

public class SessionUser : SessionBase, IRegisterAsInstancePerLifetime
{
    private readonly HttpContext _httpContext;

    public SessionUser(HttpContext httpContext)
    {
        _httpContext = httpContext;
    }

    public bool HasBetaAccess
    {
        get => (bool)_httpContext.Session["isBetaLogin"];
        set => SessionDataLegacy.Set("isBetaLogin", value);
    }

    public bool IsLoggedIn
    {
        get => SessionDataLegacy.Get("isLoggedIn", false);
        private set => SessionDataLegacy.Set("isLoggedIn", value);
    }

    //public bool IsInstallationAdmin
    //{
    //    get => SessionDataLegacy.Get("isAdministrativeLogin", false);
    //    set => SessionDataLegacy.Set("isAdministrativeLogin", value);
    //}

    //public int UserId => _userId;

    //private int _userId
    //{
    //    get => SessionDataLegacy.Get("userId", -1);
    //    set => SessionDataLegacy.Set("userId", value);
    //}

    //public SessionUserCacheItem User
    //{
    //    get
    //    {
    //        if (_userId < 0)
    //            return null;

    //        return SessionUserCache.GetUser(_userId);
    //    }
    //}

    //public static bool IsLoggedInUser(int userId)
    //{
    //    if (!IsLoggedIn)
    //        return false;

    //    return userId == UserId;
    //}

    //public static bool IsLoggedInUserOrAdmin(int userId)
    //{
    //    return IsLoggedInUser(userId) || IsInstallationAdmin;
    //}

    //public static void Login(User user)
    //{
    //    HasBetaAccess = true;
    //    IsLoggedIn = true;
    //    _userId = user.Id;
    //    CurrentWikiId = user.StartTopicId;

    //    if (user.IsInstallationAdmin)
    //        IsInstallationAdmin = true;

    //    if (HttpContext.Current != null)
    //        FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);

    //    SessionUserCache.CreateItemFromDatabase(user.Id);
    //}

    //public static void Logout()
    //{
    //    IsLoggedIn = false;
    //    IsInstallationAdmin = false;
    //    _userId = -1;
    //    CurrentWikiId = 1;

    //    if (HttpContext.Current != null)
    //        FormsAuthentication.SignOut();
    //}


    //public static List<ActivityPoints> ActivityPoints => SessionDataLegacy.Get("pointActivities", new List<ActivityPoints>());

    //public static void AddPointActivity(ActivityPoints activityPoints)
    //{
    //    ActivityPoints.Add(activityPoints);
    //}

    //public static int GetTotalActivityPoints()
    //{
    //    int totalPoints = 0;

    //    foreach (var activity in ActivityPoints)
    //        totalPoints += activity.Amount;

    //    return totalPoints;
    //}

    //public static int CurrentWikiId
    //{
    //    get => SessionDataLegacy.Get("currentWikiId", 1);
    //    private set => SessionDataLegacy.Set("currentWikiId", value);
    //}

    //public static void SetWikiId(CategoryCacheItem category) => CurrentWikiId = category.Id;
    //public static void SetWikiId(int id) => CurrentWikiId = id;

    //public static bool IsInOwnWiki() => IsLoggedIn ? CurrentWikiId == User.StartTopicId : CurrentWikiId == RootCategory.RootCategoryId;
}