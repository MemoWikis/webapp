using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using Seedworks.Web.State;
using TrueOrFalse.Utilities.ScheduledJobs;

public class SessionUser : SessionBase, IRegisterAsInstancePerLifetime
{
    public bool HasBetaAccess
    {
        get => Data.Get("isBetaLogin", false);
        set => Data["isBetaLogin"] = value;
    }

    public bool IsLoggedIn
    {
        get => Data.Get("isLoggedIn", false);
        private set => Data["isLoggedIn"] = value;
    }

    public bool IsInstallationAdmin
    {
        get => Data.Get("isAdministrativeLogin", false);
        set => Data["isAdministrativeLogin"] = value;
    }

    public User User
    {
        get => Data.Get<User>("user");
        private set => Data["user"] = value;
    }

    public bool IsLoggedInUser(int userId)
    {
        if (!IsLoggedIn)
            return false;

        return userId == User.Id;
    }

    public bool IsLoggedInUserOrAdmin(int userId)
    {
        return IsLoggedInUser(userId) || IsInstallationAdmin;
    }

    public void Login(User user)
    {
        HasBetaAccess = true;
        IsLoggedIn = true;
        User = user;
        CurrentWikiId = user.StartTopicId;

        if (user.IsInstallationAdmin)
            IsInstallationAdmin = true;

        if(HttpContext.Current != null)
            FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);

        JobScheduler.StartImmediately_InitUserValuationCache(user.Id);
    }

    public void Logout()
    {
        UserEntityCache.DeleteCacheForUser();
        IsLoggedIn = false;
        IsInstallationAdmin = false;
        User = null;
        CurrentWikiId = 1;

        if (HttpContext.Current != null)
            FormsAuthentication.SignOut();
    }

    public void UpdateUser()
    {
        User = Sl.UserRepo.GetById(Sl.SessionUser.UserId);
    }

    public int UserId
    {
        get
        {
            if (IsLoggedIn)
                return User.Id;

            return -1;
        }
    }

    public List<ActivityPoints> ActivityPoints => Data.Get("pointActivities", new List<ActivityPoints>());

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

    public int CurrentWikiId
    {
        get => Data.Get("currentWikiId", 1);
        private set => Data["currentWikiId"] = value;
    }

    public void SetWikiId(CategoryCacheItem category) => CurrentWikiId = category.Id;
    public void SetWikiId(int id) => CurrentWikiId = id;

    public bool IsInOwnWiki() => IsLoggedIn ? CurrentWikiId == User.StartTopicId : CurrentWikiId == RootCategory.RootCategoryId;
}