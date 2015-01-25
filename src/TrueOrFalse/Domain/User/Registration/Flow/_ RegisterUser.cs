using System;
using System.Data;
using NHibernate;
using NHibernate.Criterion;

namespace TrueOrFalse.Registration
{
    public class RegisterUser : IRegisterAsInstancePerLifetime
    {
        private readonly IsEmailAddressAvailable _isEmailAddressAvailable;
        private readonly UserRepository _userRepository;
        private readonly SendRegistrationEmail _sendRegistrationEmail;
        private readonly SendWelcomeMsg _sendWelcomeMsg;
        private readonly ISession _session;

        public RegisterUser(IsEmailAddressAvailable isEmailAddressAvailable, 
                            UserRepository  userRepository,
                            SendRegistrationEmail sendRegistrationEmail,
                            SendWelcomeMsg sendWelcomeMsg,
                            ISession session)
        {
            _isEmailAddressAvailable = isEmailAddressAvailable;
            _userRepository = userRepository;
            _sendRegistrationEmail = sendRegistrationEmail;
            _sendWelcomeMsg = sendWelcomeMsg;
            _session = session;
        }

        public void Run(User user)
        {
            user.Reputation = 0;
            user.ReputationPos = _userRepository.Session.QueryOver<User>()
                .Select(
                    Projections.ProjectionList()
                        .Add(Projections.Max<User>(u => u.ReputationPos)))
                .SingleOrDefault<int>() + 1;

            using(var transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                if (!_isEmailAddressAvailable.Yes(user.EmailAddress))
                    throw new Exception("There is already a user with that email address.");

                _userRepository.Create(user);
                
                transaction.Commit();
            }

            _sendRegistrationEmail.Run(user);
            _sendWelcomeMsg.Run(user);
        }
    }
}
