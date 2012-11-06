using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse
{
    public class CreatePersistentLogin : IRegisterAsInstancePerLifetime
    {
        private readonly PersistentLoginRepository _persistentLoginRepository;

        public CreatePersistentLogin(PersistentLoginRepository persistentLoginRepository)
        {
            _persistentLoginRepository = persistentLoginRepository;
        }

        public string Run(int userId)
        {
            var newGuid = Guid.NewGuid().ToString();
            var persistentLogin = new PersistentLogin { UserId = userId, LoginGuid = newGuid };
            _persistentLoginRepository.Create(persistentLogin);
            return newGuid;
        }
    }
}
