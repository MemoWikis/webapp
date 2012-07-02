using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TrueOrFalse.Core
{
    public class WritePersistentLoginToCookie : IRegisterAsInstancePerLifetime
    {
        private readonly CreatePersistentLogin _createPersistentLogin;

        public WritePersistentLoginToCookie(CreatePersistentLogin createPersistentLogin){
            _createPersistentLogin = createPersistentLogin;
        }

        public void Run(int userId)
        {
            var persistentLogin = _createPersistentLogin.Run(userId);

            var cookie = new HttpCookie("richtig-oder-falsch");
            cookie.Values.Add("persistentLogin", persistentLogin.UserId + "-x-" + persistentLogin.LoginGuid);
            cookie.Expires = DateTime.Now.AddDays(45);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }        
    }
}
