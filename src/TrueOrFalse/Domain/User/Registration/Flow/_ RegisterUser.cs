using System.Data;
using NHibernate;
using NHibernate.Criterion;

public class RegisterUser : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;
    private readonly JobQueueRepo _jobQueueRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly MessageRepo _messageRepo;
    private readonly UserWritingRepo _userWritingRepo;
    private readonly SessionUser _sessionUser;
    private readonly CategoryRepository _categoryRepository;

    public RegisterUser(ISession session,
        JobQueueRepo jobQueueRepo,
        UserReadingRepo userReadingRepo, 
        MessageRepo messageRepo,
        UserWritingRepo userWritingRepo,
        SessionUser sessionUser,
        CategoryRepository categoryRepository)
    {
        _session = session;
        _jobQueueRepo = jobQueueRepo;
        _userReadingRepo = userReadingRepo;
        _messageRepo = messageRepo;
        _userWritingRepo = userWritingRepo;
        _sessionUser = sessionUser;
        _categoryRepository = categoryRepository;
    }

    public void RegisterAndLogin(User user)
    {
        InitializeReputation(user);

        using (var transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted))
        {
            if (!IsEmailAddressAvailable.Yes(user.EmailAddress, _userReadingRepo))
                throw new Exception("There is already a user with that email address.");

            if (!IsUserNameAvailable.Yes(user.Name, _userReadingRepo))
                throw new Exception("There is already a user with that name.");

            _userWritingRepo.Create(user);
                
            transaction.Commit();
        }

        SendRegistrationEmail.Run(user, _jobQueueRepo, _userReadingRepo);
        WelcomeMsg.Send(user, _messageRepo);
        _sessionUser.Login(user);

        var category = PersonalTopic.GetPersonalCategory(user);
        category.Visibility = CategoryVisibility.Owner;
        _categoryRepository.Create(category);
        user.StartTopicId = category.Id;

        _userWritingRepo.Update(user);
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
        if (!IsEmailAddressAvailable.Yes(user.EmailAddress, _userReadingRepo))
            return new UserCreateResult { Success = false, EmailAlreadyInUse = true};

        InitializeReputation(user);

        _userWritingRepo.Create(user);

        WelcomeMsg.Send(user, _messageRepo);

        return new UserCreateResult { Success = true };
    }

    private void InitializeReputation(User user)
    {
        user.Reputation = 0;
        user.ReputationPos =
            _session.QueryOver<User>()
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