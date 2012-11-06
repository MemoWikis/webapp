using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Web.State;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Web.Context
{
    public class SessionUser : SessionBase, IRegisterAsInstancePerLifetime
    {
        public bool IsLoggedIn
        {
            get { return Data.Get<bool>("isLoggedIn", false); }
            private set { Data["isLoggedIn"] = value; }
        }

        public User User
        {
            get { return Data.Get<User>("user"); }
            private set { Data["user"] = value; }
        } 

        public void Login(User user)
        {
            IsLoggedIn = true;
            User = user;
        }

        public void Logout()
        {
            IsLoggedIn = false;
            User = null;
        }
    }
}
