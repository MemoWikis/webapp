using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TrueOrFalse.Core
{
    public class RemovePersistentLoginFromCookie : IRegisterAsInstancePerLifetime
    {
        private readonly GetPersistentLoginCookieValues _getPersistentLoginCookieValues;
        private readonly PersistentLoginRepository _persistentLoginRepository;

        public RemovePersistentLoginFromCookie(GetPersistentLoginCookieValues getPersistentLoginCookieValues,
                                               PersistentLoginRepository persistentLoginRepository)
        {
            _getPersistentLoginCookieValues = getPersistentLoginCookieValues;
            _persistentLoginRepository = persistentLoginRepository;
        }

        public void Run()
        {
            var persistentCookieValue = _getPersistentLoginCookieValues.Run();

            if (!persistentCookieValue.Exists())
                return;

            _persistentLoginRepository.Delete(persistentCookieValue.UserId, persistentCookieValue.LoginGuid);
            HttpContext.Current.Response.Cookies.Get("richtig-oder-falsch").Values.Set("persistentLogin", "");
        }
    }
}
