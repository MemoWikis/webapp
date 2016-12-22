using System;
using System.Data;
using NHibernate;
using NHibernate.Criterion;

public class RegisterUser : IRegisterAsInstancePerLifetime
{
    private readonly UserRepo _userRepo;
    private readonly ISession _session;

    public RegisterUser(UserRepo  userRepo, ISession session)
    {
        _userRepo = userRepo;
        _session = session;
    }

    public void Run(User user)
    {
        InitializeReputation(user);

        using(var transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted))
        {
            if (!IsEmailAddressAvailable.Yes(user.EmailAddress))
                throw new Exception("There is already a user with that email address.");

            if (!IsUserNameAvailable.Yes(user.Name))
                throw new Exception("There is already a user with that name.");

            _userRepo.Create(user);
                
            transaction.Commit();
        }

        SendRegistrationEmail.Run(user);
        WelcomeMsg.Send(user);
    }

    public FacebookCreateResult Run(FacebookUserCreateParameter facebookUserCreateParameter)
    {
        var user = new User();
        InitializeReputation(user);

        user.EmailAddress = facebookUserCreateParameter.email;
        user.Name = facebookUserCreateParameter.name;
        user.FacebookId = facebookUserCreateParameter.id;

        if (!IsEmailAddressAvailable.Yes(user.EmailAddress))
            return new FacebookCreateResult { Success = false };

        _userRepo.Create(user);

        WelcomeMsg.Send(user);

        return new FacebookCreateResult {Success = true};
    }

    private void InitializeReputation(User user)
    {
        user.Reputation = 0;
        user.ReputationPos =
            _userRepo.Session.QueryOver<User>()
                .Select(
                    Projections.ProjectionList()
                        .Add(Projections.Max<User>(u => u.ReputationPos)))
                .SingleOrDefault<int>() + 1;
    }
}

public class FacebookCreateResult
{
    public bool Success = false;
    public bool EmailAlreadyInUse;
}