using System;
using System.Data;
using NHibernate;
using NHibernate.Criterion;

public class RegisterUser : IRegisterAsInstancePerLifetime
{
    private readonly IsEmailAddressAvailable _isEmailAddressAvailable;
    private readonly UserRepo _userRepo;
    private readonly ISession _session;

    public RegisterUser(IsEmailAddressAvailable isEmailAddressAvailable, 
                        UserRepo  userRepo,
                        ISession session)
    {
        _isEmailAddressAvailable = isEmailAddressAvailable;
        _userRepo = userRepo;
        _session = session;
    }

    public void Run(User user)
    {
        user.Reputation = 0;
        user.ReputationPos = _userRepo.Session.QueryOver<User>()
            .Select(
                Projections.ProjectionList()
                    .Add(Projections.Max<User>(u => u.ReputationPos)))
            .SingleOrDefault<int>() + 1;

        using(var transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted))
        {
            if (!IsEmailAddressAvailable.Yes(user.EmailAddress))
                throw new Exception("There is already a user with that email address.");

            _userRepo.Create(user);
                
            transaction.Commit();
        }

        SendRegistrationEmail.Run(user);
        WelcomeMsgSend.Run(user);
    }
}