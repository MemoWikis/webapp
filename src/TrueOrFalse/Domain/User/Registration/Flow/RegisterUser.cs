﻿using NHibernate;
using NHibernate.Criterion;
using System.Data;

public class RegisterUser : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;
    private readonly JobQueueRepo _jobQueueRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly MessageRepo _messageRepo;
    private readonly UserWritingRepo _userWritingRepo;
    private readonly SessionUser _sessionUser;
    private readonly PageRepository _pageRepository;

    public RegisterUser(
        ISession session,
        JobQueueRepo jobQueueRepo,
        UserReadingRepo userReadingRepo,
        MessageRepo messageRepo,
        UserWritingRepo userWritingRepo,
        SessionUser sessionUser,
        PageRepository pageRepository)
    {
        _session = session;
        _jobQueueRepo = jobQueueRepo;
        _userReadingRepo = userReadingRepo;
        _messageRepo = messageRepo;
        _userWritingRepo = userWritingRepo;
        _sessionUser = sessionUser;
        _pageRepository = pageRepository;
    }

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
        _sessionUser.Login(user);

        var page = PersonalPage.GetPersonalCategory(user, _pageRepository);
        page.Visibility = PageVisibility.Owner;
        _pageRepository.Create(page);
        user.StartPageId = page.Id;
        user.DateCreated = DateTime.Now;
        _userWritingRepo.Update(user);
        return new RegisterResult
        {
            Success = true,
        };
    }

    public RegisterResult SetFacebookUser(FacebookUserCreateParameter facebookUser)
    {
        var user = new User
        {
            EmailAddress = facebookUser.email,
            Name = facebookUser.name,
            FacebookId = facebookUser.id,
        };

        return RegisterAndLogin(user);
    }

    public RegisterResult SetGoogleUser(GoogleUserCreateParameter googleUser)
    {
        var user = new User
        {
            EmailAddress = googleUser.Email,
            Name = googleUser.UserName,
            GoogleId = googleUser.GoogleId,
        };

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

        return RegisterAndLogin(user);
    }

    public void SendWelcomeAndRegistrationEmails(User user)
    {
        _userReadingRepo.Flush();
        _userReadingRepo.Refresh(user);

        SendRegistrationEmail.Run(user, _jobQueueRepo, _userReadingRepo);
        WelcomeMsg.Send(user.Id, _userReadingRepo, _messageRepo);
    }

    public readonly record struct CreateAndLoginResult(
        bool Success,
        string MessageKey);

    public CreateAndLoginResult CreateAndLogin(GoogleUserCreateParameter googleUser)
    {
        var registerResult = SetGoogleUser(googleUser);

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
}