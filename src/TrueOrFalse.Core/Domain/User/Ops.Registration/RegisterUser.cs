using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core
{
    public class RegisterUser
    {
        private readonly IsUserNameAvailable _isUserNameAvailable;
        private readonly UserRepository _userRepository;

        public RegisterUser(IsUserNameAvailable isUserNameAvailable, 
                            UserRepository  userRepository)
        {
            _isUserNameAvailable = isUserNameAvailable;
            _userRepository = userRepository;
        }

        public void Run(User user)
        {
            //Send Email
            //SetUserStatus
            
            //In Transaction
                //Validated that user name is not unique
            //End Transation

            //Persist
        }

    }
}
