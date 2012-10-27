using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NHibernate;
using TrueOrFalse.Core.Infrastructure;

namespace TrueOrFalse.Core.Registration
{
    public class RegisterUser : IRegisterAsInstancePerLifetime
    {
        private readonly IsEmailAddressNotInUse _isEmailAddressNotInUse;
        private readonly UserRepository _userRepository;
        private readonly SendRegistrationEmail _sendRegistrationEmail;
        private readonly ISession _session;

        public RegisterUser(IsEmailAddressNotInUse isEmailAddressNotInUse, 
                            UserRepository  userRepository,
                            SendRegistrationEmail sendRegistrationEmail,
                            ISession session)
        {
            _isEmailAddressNotInUse = isEmailAddressNotInUse;
            _userRepository = userRepository;
            _sendRegistrationEmail = sendRegistrationEmail;
            _session = session;
        }

        public void Run(User user)
        {
            using(var transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                if (!_isEmailAddressNotInUse.Yes(user.EmailAddress))
                    throw new Exception("There is already a user with that email address.");

                _userRepository.Create(user);
                
                transaction.Commit();
            }

            _sendRegistrationEmail.Run(user);
        }

    }
}
