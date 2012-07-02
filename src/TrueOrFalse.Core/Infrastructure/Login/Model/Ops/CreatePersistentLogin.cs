using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core
{
    public class CreatePersistentLogin : IRegisterAsInstancePerLifetime
    {
        private readonly PersistentLoginRepository _persistentLoginRepository;

        public CreatePersistentLogin(PersistentLoginRepository persistentLoginRepository)
        {
            _persistentLoginRepository = persistentLoginRepository;
        }

        public PersistentLogin Run(int userId)
        {
            var persistentLogin = new PersistentLogin{UserId = userId, LoginGuid = Guid.NewGuid().ToString()};
            _persistentLoginRepository.Create(persistentLogin);
            return persistentLogin;
        }
    }
}
