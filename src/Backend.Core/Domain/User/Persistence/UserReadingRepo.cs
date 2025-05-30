﻿using ISession = NHibernate.ISession;

public class UserReadingRepo : RepositoryDb<User>
{
    public UserReadingRepo(ISession session) : base(session)
    {
    }

    public bool FacebookUserExists(string facebookId)
    {
        return Session
            .QueryOver<User>()
            .Where(u => u.FacebookId == facebookId)
            .RowCount() == 1;
    }

    public IList<User> GetAll()
    {
        return base.GetAll();
    }

    public IList<int> GetAllIds()
    {
        return base.GetAllIds();
    }

    public User GetByEmail(string emailAddress)
    {
        return Session.QueryOver<User>()
            .Where(k => k.EmailAddress == emailAddress)
            .SingleOrDefault<User>();
    }

    public IList<User> GetByIds(params int[] userIds)
    {
        var users = base.GetByIds(userIds);

        if (userIds.Length != users.Count)
        {
            var missingUsersIds = userIds.Where(id => !users.Any(u => id == u.Id)).ToList();
            Log.Error(
                $"Following user ids from meilisearch not found: {string.Join(",", missingUsersIds.OrderBy(id => id))}");
        }

        return userIds.Select(t => users.FirstOrDefault(u => u.Id == t)).Where(x => x != null)
            .ToList();
    }

    public User GetByName(string name)
    {
        return Session.QueryOver<User>()
            .Where(k => k.Name == name)
            .SingleOrDefault<User>();
    }

    public User GetByStripeId(string stripId)
    {
        if (stripId == null)
        {
            return null;
        }

        return Session.QueryOver<User>()
            .Where(u => u.StripeId == stripId)
            .SingleOrDefault();
    }

    public IList<User> GetWhereReputationIsBetween(int newReputation, int oldRepution)
    {
        var query = Session.QueryOver<User>();

        if (newReputation < oldRepution)
        {
            query.Where(u => u.Reputation > newReputation && u.Reputation < oldRepution);
        }
        else
        {
            query.Where(u => u.Reputation < newReputation && u.Reputation > oldRepution);
        }

        return query.List();
    }

    public bool GoogleUserExists(string googleId)
    {
        return Session
            .QueryOver<User>()
            .Where(u => u.GoogleId == googleId)
            .RowCount() == 1;
    }

    public User? UserGetByFacebookId(string facebookId)
    {
        return Session
            .QueryOver<User>()
            .Where(u => u.FacebookId == facebookId)
            .SingleOrDefault();
    }

    public User? UserGetByGoogleId(string googleId)
    {
        return Session
            .QueryOver<User>()
            .Where(u => u.GoogleId == googleId)
            .SingleOrDefault();
    }
}