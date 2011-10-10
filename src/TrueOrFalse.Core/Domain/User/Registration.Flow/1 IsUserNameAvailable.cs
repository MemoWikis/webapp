using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Core.Infrastructure;

namespace TrueOrFalse.Core.Registration
{
    public class IsEmailAddressNotInUse : IRegisterAsInstancePerLifetime
    {
        private readonly UserRepository _userRepository;

        public IsEmailAddressNotInUse(UserRepository userRepository){
            _userRepository = userRepository;
        }

        public bool Yes(string emailAddress)
        {
            return _userRepository.GetByEmailAddress(emailAddress) == null;
        }
    }
}
