using NHibernate;
using NHibernate.Criterion;
using System.Data;
using TrueOrFalse.Search;

public class RegisterUser(
    ISession _session,
    JobQueueRepo _jobQueueRepo,
    UserReadingRepo _userReadingRepo,
    MessageRepo _messageRepo,
    UserWritingRepo _userWritingRepo,
    SessionUser _sessionUser,
    PageRepository _pageRepository,
    PageViewRepo _pageViewRepo,
    MeiliSearchReIndexUser _meiliSearchReIndexUser)
    : IRegisterAsInstancePerLifetime
{
    public readonly record struct RegisterResult(bool Success, string MessageKey);

    private RegisterResult RegisterAndLogin(User user)
    {
        using (var transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted))
        {
            if (IsEmailAddressAvailable.No(user.EmailAddress, _userReadingRepo))
                return new RegisterResult
                {
                    Success = false,
                    MessageKey = FrontendMessageKeys.Error.User.EmailInUse
                };

            if (IsEmailAdressFormat.NotValid(user.EmailAddress))
                return new RegisterResult
                {
                    Success = false,
                    MessageKey = FrontendMessageKeys.Error.User.FalseEmailFormat
                };

            if (!user.IsFacebookUser &&
                !user.IsGoogleUser &&
                IsUserNameAvailable.No(user.Name, _userReadingRepo))
                return new RegisterResult
                {
                    Success = false,
                    MessageKey = FrontendMessageKeys.Error.User.UserNameInUse
                };

            InitializeReputation(user);
            _userWritingRepo.Create(user);

            transaction.Commit();
        }

        SendRegistrationEmail.Run(user, _jobQueueRepo, _userReadingRepo);
        WelcomeMsg.Send(user, _messageRepo);
        _sessionUser.Login(user, _pageViewRepo);

        var page = PersonalPage.GetPersonalPage(user, _pageRepository);
        page.Visibility = PageVisibility.Owner;
        _pageRepository.Create(page);
        user.StartPageId = page.Id;
        user.DateCreated = DateTime.Now;
        var userCacheItem = EntityCache.GetUserById(user.Id);
        LanguageExtensions.AddContentLanguageToUser(userCacheItem, user.UiLanguage);
        _userWritingRepo.Update(user);

        return new RegisterResult
        {
            Success = true,
        };
    }

    public RegisterResult SetFacebookUser(FacebookUserCreateParameter facebookUser, string language)
    {
        var user = new User
        {
            EmailAddress = facebookUser.email,
            Name = facebookUser.name,
            FacebookId = facebookUser.id,
        };

        if (LanguageExtensions.CodeExists(language))
            user.UiLanguage = language;

        return RegisterAndLogin(user);
    }

    public RegisterResult SetGoogleUser(GoogleUserCreateParameter googleUser, string language)
    {
        var user = new User
        {
            EmailAddress = googleUser.Email,
            Name = googleUser.UserName,
            GoogleId = googleUser.GoogleId,
        };

        if (LanguageExtensions.CodeExists(language))
            user.UiLanguage = language;

        return RegisterAndLogin(user);
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

    public RegisterResult SetUser(RegisterJson json)
    {
        var user = new User();
        user.EmailAddress = json.Email.TrimAndReplaceWhitespacesWithSingleSpace();
        user.Name = json.Name.TrimAndReplaceWhitespacesWithSingleSpace();
        SetUserPassword.Run(json.Password.Trim(), user);

        if (LanguageExtensions.CodeExists(json.Language))
            user.UiLanguage = json.Language;

        return RegisterAndLogin(user);
    }

    public readonly record struct CreateAndLoginResult(
        bool Success,
        string MessageKey);

    public CreateAndLoginResult CreateAndLogin(GoogleUserCreateParameter googleUser, string language)
    {
        var registerResult = SetGoogleUser(googleUser, language);

        if (registerResult.Success)

        {
            return new CreateAndLoginResult
            {
                Success = true,
            };
        }

        return new CreateAndLoginResult
        {
            Success = false,
            MessageKey = registerResult.MessageKey
        };
    }
}

public class RegisterJson
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Language { get; set; }
}