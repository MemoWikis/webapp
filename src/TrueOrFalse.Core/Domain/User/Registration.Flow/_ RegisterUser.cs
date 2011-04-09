using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NHibernate;

namespace TrueOrFalse.Core.Registration
{
    public class RegisterUser
    {
        private readonly IsUserNameAvailable _isUserNameAvailable;
        private readonly UserRepository _userRepository;
        private readonly SendRegistrationEmail _sendRegistrationEmail;
        private readonly ISession _session;

        public RegisterUser(IsUserNameAvailable isUserNameAvailable, 
                            UserRepository  userRepository,
                            SendRegistrationEmail sendRegistrationEmail,
                            ISession session)
        {
            _isUserNameAvailable = isUserNameAvailable;
            _userRepository = userRepository;
            _sendRegistrationEmail = sendRegistrationEmail;
            _session = session;
        }

        public void Run(User user)
        {
            using(var transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                if (!_isUserNameAvailable.Yes(user.UserName))
                    throw new Exception("the username is not available anymore");

                _userRepository.Create(user);
                
                transaction.Commit();
            }

            _sendRegistrationEmail.Run(user);
        }

    }
}
