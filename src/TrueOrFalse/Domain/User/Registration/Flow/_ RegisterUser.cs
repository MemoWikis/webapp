using System.Data;
using System.Web.Helpers;
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

    private (bool success, string message) RegisterAndLogin(User user)
    {
        InitializeReputation(user);
        using (var transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted))
        {
            if (!IsEmailAddressAvailable.Yes(user.EmailAddress, _userReadingRepo))
                return (false, "emailInUse");

            if (!user.IsFacebookUser &&
                !user.IsGoogleUser &&
                !IsUserNameAvailable.Yes(user.Name, _userReadingRepo))
                return (false, "userNameInUse");

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
        return (true, ""); 
    }

    public bool SetFacebookUser(FacebookUserCreateParameter facebookUser)
    {
        var user = new User
        {
            EmailAddress = facebookUser.email,
            Name = facebookUser.name,
            FacebookId = facebookUser.id
        };

        return RegisterAndLogin(user).success;
    }

    public bool SetGoogleUser(GoogleUserCreateParameter googleUser)
    {
        var user = new User
        {
            EmailAddress = googleUser.Email,
            Name = googleUser.UserName,
            GoogleId = googleUser.GoogleId
        };

        return RegisterAndLogin(user).success;
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
    public (bool success, string message) SetUser(RegisterJson json)
    {
        var user = new User();
        user.EmailAddress = json.Email.TrimAndReplaceWhitespacesWithSingleSpace();
        user.Name = json.Name.TrimAndReplaceWhitespacesWithSingleSpace();
        SetUserPassword.Run(json.Password.Trim(), user);

        return RegisterAndLogin(user);
    }
}

public class UserCreateResult
{
    public bool Success = false;
    public bool EmailAlreadyInUse;
}

public class RegisterJson
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}