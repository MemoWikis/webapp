using System;
using System.Web;

namespace TrueOrFalse.Core
{
    public class GetPersistentLoginCookieValues : IRegisterAsInstancePerLifetime
    {
        public GetPersistentLoginFromCookie Run()
        {
            var cookie = HttpContext.Current.Request.Cookies.Get("richtig-oder-falsch");

            if (cookie == null)
                return new GetPersistentLoginFromCookie();

            var valuePersistentLogin = cookie["persistentLogin"];
            if (string.IsNullOrEmpty(valuePersistentLogin))
                return new GetPersistentLoginFromCookie();

            var item = valuePersistentLogin.Split(new[] { "-x-" }, StringSplitOptions.None);

            return new GetPersistentLoginFromCookie
                       {
                           UserId = Convert.ToInt32(item[0]),
                           LoginGuid = item[1]
                       };
        }
    }
}
