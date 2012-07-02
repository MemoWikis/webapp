using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core
{
    public class LoginFromCookie : IRegisterAsInstancePerLifetime
    {
        private readonly GetPersistentLoginCookieValues _getPersistentLoginCookieValues;
        private readonly PersistentLoginRepository _persistentLoginRepository;

        public LoginFromCookie(GetPersistentLoginCookieValues getPersistentLoginCookieValues, 
                               PersistentLoginRepository persistentLoginRepository)
        {
            _getPersistentLoginCookieValues = getPersistentLoginCookieValues;
            _persistentLoginRepository = persistentLoginRepository;
        }

        public bool Run()
        {
            var cookieValues = _getPersistentLoginCookieValues.Run();

            if (!cookieValues.Exists())
                return false;

            var persistentLogin = _persistentLoginRepository.Get(cookieValues.UserId, cookieValues.LoginGuid);

            if (persistentLogin == null)
                return false;



            return true;
        }
    }
}
