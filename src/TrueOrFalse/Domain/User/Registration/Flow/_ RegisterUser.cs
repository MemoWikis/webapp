using System;
using System.Data;
using NHibernate.Criterion;

public class RegisterUser : IRegisterAsInstancePerLifetime
{
    public static void Run(User user)
    {
        InitializeReputation(user);

        var userRepo = Sl.R<UserRepo>();

        using (var transaction = userRepo.Session.BeginTransaction(IsolationLevel.ReadCommitted))
        {
            if (!IsEmailAddressAvailable.Yes(user.EmailAddress))
                throw new Exception("There is already a user with that email address.");

            if (!IsUserNameAvailable.Yes(user.Name))
                throw new Exception("There is already a user with that name.");

            userRepo.Create(user);
                
            transaction.Commit();
        }

        SendRegistrationEmail.Run(user);
        WelcomeMsg.Send(user);
    }

    public static UserCreateResult Run(FacebookUserCreateParameter facebookUser)
    {
        var user = new User
        {
            EmailAddress = facebookUser.email,
            Name = facebookUser.name,
            FacebookId = facebookUser.id
        };

        return Register(user);
    }

    public static UserCreateResult Run(GoogleUserCreateParameter googleUser)
    {
        var user = new User
        {
            EmailAddress = googleUser.Email,
            Name = googleUser.UserName,
            GoogleId = googleUser.GoogleId
        };

        return Register(user);
    }

    private static UserCreateResult Register(User user)
    {
        if (!IsEmailAddressAvailable.Yes(user.EmailAddress))
            return new UserCreateResult { Success = false, EmailAlreadyInUse = true};

        InitializeReputation(user);

        Sl.UserRepo.Create(user);

        WelcomeMsg.Send(user);

        return new UserCreateResult { Success = true };
    }

    private static void InitializeReputation(User user)
    {
        user.Reputation = 0;
        user.ReputationPos =
            Sl.Session.QueryOver<User>()
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