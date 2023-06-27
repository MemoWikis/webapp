using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using Seedworks.Web.State;

public class SessionUserLegacy : IRegisterAsInstancePerLifetime
{
    private readonly SessionData _sessionData;


    public SessionUserLegacy(SessionData sessionData)
    {
        _sessionData = sessionData;
    }

    public bool HasBetaAccess
    {
        get => _sessionData.Get("isBetaLogin", false);
        set => _sessionData.Set("isBetaLogin", value);
    }

    public bool IsLoggedIn
    {
        get => _sessionData.Get("isLoggedIn", false);
        private set => _sessionData.Set("isLoggedIn", value);
    }

    public bool IsInstallationAdmin
    {
        get => _sessionData.Get("isAdministrativeLogin", false);
        set => _sessionData.Set("isAdministrativeLogin", value);
    }

    public int UserId => _userId;
    
    private int _userId
    {
        get => _sessionData.Get("userId", -1);
        set => _sessionData.Set("userId", value);
    }

    public SessionUserCacheItem User
    {
        get
        {
            if (_userId < 0) 
                return null;

            return SessionUserCache.GetUser(_userId);
        }
    }

    public bool IsLoggedInUser(int userId)
    {
        if (!IsLoggedIn)
            return false;

        return userId == UserId;
    }

    public bool IsLoggedInUserOrAdmin(int userId)
    {
        return IsLoggedInUser(userId) || IsInstallationAdmin;
    }

    public void Login(User user)
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

    public void Logout()
    {
        IsLoggedIn = false;
        IsInstallationAdmin = false;
        _userId = -1;
        CurrentWikiId = 1;

        if (HttpContext.Current != null)
            FormsAuthentication.SignOut();
    }


    public List<ActivityPoints> ActivityPoints => _sessionData.Get("pointActivities", new List<ActivityPoints>());

    public int CurrentWikiId
    {
        get => _sessionData.Get("currentWikiId", 1);
        private set => _sessionData.Set("currentWikiId", value);
    }

    public void SetWikiId(CategoryCacheItem category) => CurrentWikiId = category.Id;
    public void SetWikiId(int id) => CurrentWikiId = id;
}