using System.Web;
using System.Web.Security;
using Seedworks.Web.State;

public class SessionUser : SessionBase, IRegisterAsInstancePerLifetime
{
    public bool HasBetaAccess
    {
        get { return Data.Get("isBetaLogin", false); }
        set { Data["isBetaLogin"] = value; }
    }

    public LearningSession LearningSession
    {
        get { return Data.Get<LearningSession>("learningSession"); }
        set { Data["learningSession"] = value; }
    }

    public bool IsLoggedIn
    {
        get { return Data.Get("isLoggedIn", false); }
        private set { Data["isLoggedIn"] = value; }
    }

    public bool IsInstallationAdmin
    {
        get { return Data.Get("isAdministrativeLogin", false); }
        set { Data["isAdministrativeLogin"] = value; }
    } 

    public User User
    {
        get { return Data.Get<User>("user"); }
        private set { Data["user"] = value; }
    }

    public bool IsValidUser(int userId)
    {
        if (!IsLoggedIn)
            return false;

        return userId == User.Id;
    }

    public bool IsValidUserOrAdmin(int userId)
    {
        return IsValidUser(userId) || IsInstallationAdmin;
    }

    public void Login(User user)
    {
        HasBetaAccess = true;
        IsLoggedIn = true;
        User = user;

        if (user.IsInstallationAdmin)
            IsInstallationAdmin = true;

        if(HttpContext.Current != null)
            FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
    }

    public void Logout()
    {
        IsLoggedIn = false;
        User = null;
        if (HttpContext.Current != null)
            FormsAuthentication.SignOut();
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
}