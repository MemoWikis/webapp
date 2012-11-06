using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Registration
{
    public class CredentialsAreValid : IRegisterAsInstancePerLifetime
    {
        private readonly UserRepository _userRepository;
        private readonly IsValdidPassword _isValidPassword;
        
        public User User;

        public CredentialsAreValid(UserRepository userRepository, 
                                   IsValdidPassword isValidPassword)
        {
            _userRepository = userRepository;
            _isValidPassword = isValidPassword;
        }

        public bool Yes(string emailAdress, string password)
        {

            if (emailAdress.Trim().Length == 0 || password.Trim().Length == 0)
                return false;

            User = null;
            var user = _userRepository.GetByEmail(emailAdress.Trim());

            if (user == null)
                return false;

            var isValidPassword =  _isValidPassword.True(password, user);

            if (isValidPassword)
                User = user;

            return isValidPassword;
        }
    }
}
