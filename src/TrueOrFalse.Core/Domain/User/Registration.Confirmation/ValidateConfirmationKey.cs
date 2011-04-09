using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core.Registration
{
    public class ValidateEmailConfirmationKey
    {
        private readonly UserRepository _userRepository;

        public ValidateEmailConfirmationKey(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool IsValid(string affirmationKey)
        {
            if (affirmationKey.Length <= 4)
                return false;

            int userId;
            if (!Int32.TryParse(affirmationKey.Substring(3), out userId) == false)
                return false;

            if (_userRepository.GetById(userId) != null)
                return true;

            return true;
        }
    }
}
