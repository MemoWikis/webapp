using System.Data;
using NHibernate;
using NHibernate.Criterion;

public class RegisterUser : IRegisterAsInstancePerLifetime
{
    private readonly ISession _nhibernateSession;
    private readonly JobQueueRepo _jobQueueRepo;
    private readonly UserRepo _userRepo;
    private readonly MessageRepo _messageRepo;

    public RegisterUser(ISession nhibernateSession,
        JobQueueRepo jobQueueRepo,
        UserRepo userRepo, 
        MessageRepo messageRepo)
    {
        _nhibernateSession = nhibernateSession;
        _jobQueueRepo = jobQueueRepo;
        _userRepo = userRepo;
        _messageRepo = messageRepo;
    }

    public void Run(User user)
    {
        InitializeReputation(user);

     

        using (var transaction = _userRepo.Session.BeginTransaction(IsolationLevel.ReadCommitted))
        {
            if (!IsEmailAddressAvailable.Yes(user.EmailAddress, _userRepo))
                throw new Exception("There is already a user with that email address.");

            if (!IsUserNameAvailable.Yes(user.Name, _userRepo))
                throw new Exception("There is already a user with that name.");

            _userRepo.Create(user);
                
            transaction.Commit();
        }

        SendRegistrationEmail.Run(user, _jobQueueRepo, _userRepo);
        WelcomeMsg.Send(user, _messageRepo);
    }

    public UserCreateResult Run(FacebookUserCreateParameter facebookUser)
    {
        var user = new User
        {
            EmailAddress = facebookUser.email,
            Name = facebookUser.name,
            FacebookId = facebookUser.id
        };

        return Register(user);
    }

    public UserCreateResult Run(GoogleUserCreateParameter googleUser)
    {
        var user = new User
        {
            EmailAddress = googleUser.Email,
            Name = googleUser.UserName,
            GoogleId = googleUser.GoogleId
        };

        return Register(user);
    }

    private UserCreateResult Register(User user)
    {
        if (!IsEmailAddressAvailable.Yes(user.EmailAddress, _userRepo))
            return new UserCreateResult { Success = false, EmailAlreadyInUse = true};

        InitializeReputation(user);

        _userRepo.Create(user);

        WelcomeMsg.Send(user, _messageRepo);

        return new UserCreateResult { Success = true };
    }

    private void InitializeReputation(User user)
    {
        user.Reputation = 0;
        user.ReputationPos =
            _nhibernateSession.QueryOver<User>()
                .Select(
                    Projections.ProjectionList()
                        .Add(Projections.Max<User>(u => u.ReputationPos)))
                .SingleOrDefault<int>() + 1;
    }
}

public class UserCreateResult
{
    public bool Success = false;
    public bool EmailAlreadyInUse;
}