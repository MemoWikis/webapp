using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using Seedworks.Web.State;

public class SessionUser : SessionBase, IRegisterAsInstancePerLifetime
{
    public static bool HasBetaAccess
    {
        get => SessionData.Get("isBetaLogin", false);
        set => SessionData.Set("isBetaLogin", value);
    }

    public static bool IsLoggedIn
    {
        get => SessionData.Get("isLoggedIn", false);
        private set => SessionData.Set("isLoggedIn", value);
    }

    public static bool IsInstallationAdmin
    {
        get => SessionData.Get("isAdministrativeLogin", false);
        set => SessionData.Set("isAdministrativeLogin", value);
    }

    private static int _userId
    {
        get => SessionData.Get("userId", -1);
        set => SessionData.Set("userId", value);
    }

    public static SessionUserCacheItem User
    {
        get
        {
            if (_userId == -1) 
                return null;

            return SessionUserCache.GetUser(_userId);
        }
    }

    public static bool IsLoggedInUser(int userId)
    {
        if (!IsLoggedIn)
            return false;

        return userId == User.Id;
    }

    public static bool IsLoggedInUserOrAdmin(int userId)
    {
        return IsLoggedInUser(userId) || IsInstallationAdmin;
    }

    public static void Login(User user)
    {
        HasBetaAccess = true;
        IsLoggedIn = true;
        _userId = user.Id;
        CurrentWikiId = user.StartTopicId;

        if (user.IsInstallationAdmin)
            IsInstallationAdmin = true;

        if (HttpContext.Current != null)
            FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);

        SessionUserCache.CreateItemFromDatabase(user.Id);
    }

    public static void Logout()
    {
        IsLoggedIn = false;
        IsInstallationAdmin = false;
        _userId = -1;
        CurrentWikiId = 1;

        if (HttpContext.Current != null)
            FormsAuthentication.SignOut();
    }

    public static int UserId => _userId;

    public static List<ActivityPoints> ActivityPoints => SessionData.Get("pointActivities", new List<ActivityPoints>());

    public static void AddPointActivity(ActivityPoints activityPoints)
    {
        ActivityPoints.Add(activityPoints);
    }

    public static int GetTotalActivityPoints()
    {
        int totalPoints = 0;

        foreach (var activity in ActivityPoints)
            totalPoints += activity.Amount;

        return totalPoints;
    }

    public static int CurrentWikiId
    {
        get => SessionData.Get("currentWikiId", 1);
        private set => SessionData.Set("currentWikiId", value);
    }

    public static void SetWikiId(CategoryCacheItem category) => CurrentWikiId = category.Id;
    public static void SetWikiId(int id) => CurrentWikiId = id;

    public static bool IsInOwnWiki() => IsLoggedIn ? CurrentWikiId == User.StartTopicId : CurrentWikiId == RootCategory.RootCategoryId;
}