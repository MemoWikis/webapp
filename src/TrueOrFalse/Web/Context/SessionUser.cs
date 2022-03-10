using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using Seedworks.Web.State;
using TrueOrFalse.Utilities.ScheduledJobs;

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

    public static User User
    {
        get => SessionData.Get<User>("user");
        private set => SessionData.Set("user", value);
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
        User = user;
        CurrentWikiId = user.StartTopicId;

        if (user.IsInstallationAdmin)
            IsInstallationAdmin = true;

        if (HttpContext.Current != null)
            FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);

        JobScheduler.StartImmediately_InitUserValuationCache(user.Id);
    }

    public static void Logout()
    {
        UserEntityCache.DeleteCacheForUser();
        IsLoggedIn = false;
        IsInstallationAdmin = false;
        User = null;
        CurrentWikiId = 1;

        if (HttpContext.Current != null)
            FormsAuthentication.SignOut();
    }

    public static void UpdateUser()
    {
        User = Sl.UserRepo.GetById(UserId);
    }

    public static int UserId
    {
        get
        {
            if (IsLoggedIn)
                return User.Id;

            return -1;
        }
    }

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