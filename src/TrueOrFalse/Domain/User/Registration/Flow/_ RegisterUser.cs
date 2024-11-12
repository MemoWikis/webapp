//using System.Data;
//using NHibernate;
//using NHibernate.Criterion;

//public class RegisterUserOld : IRegisterAsInstancePerLifetime
//{
//    private readonly ISession _nhibernateSession;

//    public RegisterUserOld(ISession nhibernateSession)
//    {
//        _nhibernateSession = nhibernateSession;
//    }

//    public void Run(User user)
//    {
//        InitializeReputation(user);

//        var userRepo = Sl.R<UserRepo>();

//        using (var transaction = userRepo.Session.BeginTransaction(IsolationLevel.ReadCommitted))
//        {
//            if (!IsEmailAddressAvailable.Yes(user.EmailAddress))
//                throw new Exception("There is already a user with that email address.");

//            if (!IsUserNameAvailable.Yes(user.Name))
//                throw new Exception("There is already a user with that name.");

//            userRepo.Create(user);
                
//            transaction.Commit();
//        }
//    }

//    public void CreateStartPageAndSetToUser(User user)
//    {
//        var topic = PersonalPage.GetPersonalCategory(user);
//        Sl.CategoryRepo.Create(topic);
//        user.StartPageId = topic.Id;

//        Sl.UserRepo.Update(user);
//    }

//    public UserCreateResult Run(FacebookUserCreateParameter facebookUser)
//    {
//        var user = new User
//        {
//            EmailAddress = facebookUser.email,
//            Name = facebookUser.name,
//            FacebookId = facebookUser.id
//        };

//        return Register(user);
//    }

//    public UserCreateResult Run(GoogleUserCreateParameter googleUser)
//    {
//        var user = new User
//        {
//            EmailAddress = googleUser.Email,
//            Name = googleUser.UserName,
//            GoogleId = googleUser.GoogleId
//        };

//        return Register(user);
//    }

//    private UserCreateResult Register(User user)
//    {
//        if (!IsEmailAddressAvailable.Yes(user.EmailAddress))
//            return new UserCreateResult { Success = false, EmailAlreadyInUse = true};

//        InitializeReputation(user);

//        Sl.UserRepo.Create(user);

//        return new UserCreateResult { Success = true };
//    }

//    private void InitializeReputation(User user)
//    {
//        user.Reputation = 0;
//        user.ReputationPos =
//            _nhibernateSession.QueryOver<User>()
//                .Select(
//                    Projections.ProjectionList()
//                        .Add(Projections.Max<User>(u => u.ReputationPos)))
//                .SingleOrDefault<int>() + 1;
//    }

//    public void SendWelcomeAndRegistrationEmails(User user)
//    {
//        var userRepo = Sl.R<UserRepo>();
//        userRepo.Flush();
//        userRepo.Refresh(user);

//        SendRegistrationEmail.Run(user);
//        WelcomeMsg.Send(user);
//    }
//}

//public class UserCreateResult
//{
//    public bool Success = false;
//    public bool EmailAlreadyInUse;
//}