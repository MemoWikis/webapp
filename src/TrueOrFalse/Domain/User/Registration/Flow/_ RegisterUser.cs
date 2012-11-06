using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NHibernate;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Registration
{
    public class RegisterUser : IRegisterAsInstancePerLifetime
    {
        private readonly IsEmailAddressAvailable _isEmailAddressAvailable;
        private readonly UserRepository _userRepository;
        private readonly SendRegistrationEmail _sendRegistrationEmail;
        private readonly ISession _session;

        public RegisterUser(IsEmailAddressAvailable isEmailAddressAvailable, 
                            UserRepository  userRepository,
                            SendRegistrationEmail sendRegistrationEmail,
                            ISession session)
        {
            _isEmailAddressAvailable = isEmailAddressAvailable;
            _userRepository = userRepository;
            _sendRegistrationEmail = sendRegistrationEmail;
            _session = session;
        }

        public void Run(User user)
        {
            using(var transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                if (!_isEmailAddressAvailable.Yes(user.EmailAddress))
                    throw new Exception("There is already a user with that email address.");

                _userRepository.Create(user);
                
                transaction.Commit();
            }

            _sendRegistrationEmail.Run(user);
        }

    }
}
