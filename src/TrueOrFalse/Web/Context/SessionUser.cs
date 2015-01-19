using Seedworks.Web.State;

namespace TrueOrFalse.Web.Context
{
    public class SessionUser : SessionBase, IRegisterAsInstancePerLifetime
    {
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
            IsLoggedIn = true;
            User = user;

            if (user.IsInstallationAdmin)
                IsInstallationAdmin = true;
        }

        public void Logout()
        {
            IsLoggedIn = false;
            User = null;
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
}
