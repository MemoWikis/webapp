using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Core.Infrastructure;

namespace TrueOrFalse.Core
{
    public class IsUserNameAvailable : IRegisterAsInstancePerLifetime
    {
        private readonly UserRepository _userRepository;

        public IsUserNameAvailable(UserRepository userRepository){
            _userRepository = userRepository;
        }

        public bool Run(string userName)
        {
            return (_userRepository.GetByUserName(userName) != null);
        }
    }
}
