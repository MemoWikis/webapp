using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Core.Infrastructure;

namespace TrueOrFalse.Core.Registration
{
    public class CredentialsAreValid : IRegisterAsInstancePerLifetime
    {
        private readonly UserRepository _userRepository;
        private readonly IsValdidPassword _isValidPassword;

        public CredentialsAreValid(UserRepository userRepository, 
                                   IsValdidPassword isValidPassword)
        {
            _userRepository = userRepository;
            _isValidPassword = isValidPassword;
        }

        public bool Yes(string userName, string password)
        {
            var user = _userRepository.GetByUserName(userName.Trim());

            if (user == null)
                return false;

            return _isValidPassword.True(password, user);
        }
    }
}
