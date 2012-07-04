using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Core.Web.Context;

namespace TrueOrFalse.Core
{
    public class LoginFromCookie : IRegisterAsInstancePerLifetime
    {
        private readonly GetPersistentLoginCookieValues _getPersistentLoginCookieValues;
        private readonly WritePersistentLoginToCookie _writePersistentLoginToCookie;
        private readonly PersistentLoginRepository _persistentLoginRepository;
        private readonly SessionUser _sessionUser;
        private readonly UserRepository _userRepository;

        public LoginFromCookie(GetPersistentLoginCookieValues getPersistentLoginCookieValues,
                               WritePersistentLoginToCookie writePersistentLoginToCookie,
                               PersistentLoginRepository persistentLoginRepository, 
                               SessionUser sessionUser, 
                               UserRepository userRepository)
        {
            _getPersistentLoginCookieValues = getPersistentLoginCookieValues;
            _writePersistentLoginToCookie = writePersistentLoginToCookie;
            _persistentLoginRepository = persistentLoginRepository;
            _sessionUser = sessionUser;
            _userRepository = userRepository;
        }

        public bool Run()
        {
            var cookieValues = _getPersistentLoginCookieValues.Run();

            if (!cookieValues.Exists())
                return false;

            var persistentLogin = _persistentLoginRepository.Get(cookieValues.UserId, cookieValues.LoginGuid);

            if (persistentLogin == null)
                return false;

            var user = _userRepository.GetById(cookieValues.UserId);
            if (user == null)
                return false;

            _persistentLoginRepository.Delete(persistentLogin);
            _writePersistentLoginToCookie.Run(cookieValues.UserId);

            _sessionUser.Login(user);            

            return true;
        }
    }
}
