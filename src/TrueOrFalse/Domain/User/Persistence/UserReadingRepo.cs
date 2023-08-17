using NHibernate;
using Seedworks.Lib.Persistence;

public class UserReadingRepo : RepositoryDbBase<UserReadingRepo>
{
    private readonly RepositoryDb<User> _repo;
    public int Id { get; set; }

    public UserReadingRepo(ISession session) : base(session)
    {
        _repo = new RepositoryDb<User>(session); 
    }

    public bool FacebookUserExists(string facebookId)
    {
        return _repo.Session
            .QueryOver<User>()
            .Where(u => u.FacebookId == facebookId)
            .RowCount() == 1;
    }

    public IList<User> GetAll()
    {
        return _repo.GetAll(); 
    }

    public IList<int> GetAllIds()
    {
        return _repo.GetAllIds(); 
    }

    public User GetByEmail(string emailAddress)
    {
        return _repo.Session.QueryOver<User>()
            .Where(k => k.EmailAddress == emailAddress)
            .SingleOrDefault<User>();
    }

    public User GetByEmailEager(string email)
    {
        var user = _repo.Session
            .QueryOver<User>()
            .Where(u => u.EmailAddress == email)
            .SingleOrDefault();

        _repo.Session
            .QueryOver<User>()
            .Where(u => u.EmailAddress == email)
            .Fetch(SelectMode.Fetch, u => u.Followers)
            .SingleOrDefault();

        _repo.Session
            .QueryOver<User>()
            .Where(u => u.EmailAddress == email)
            .Fetch(SelectMode.Fetch, u => u.Following)
            .SingleOrDefault();
        return user;
    }

    public IList<User> GetByIds(params int[] userIds)
    {
        var users = _repo.GetByIds(userIds);

        if (userIds.Length != users.Count)
        {
            var missingUsersIds = userIds.Where(id => !users.Any(u => id == u.Id)).ToList();
            new Logg(_httpContextAccessor, _webHostEnvironment).r().Error(
                $"Following user ids from meilisearch not found: {string.Join(",", missingUsersIds.OrderBy(id => id))}");
        }

        return userIds.Select(t => users.FirstOrDefault(u => u.Id == t)).Where(x => x != null).ToList();
    }

    public User GetByName(string name)
    {
        return _repo.Session.QueryOver<User>()
            .Where(k => k.Name == name)
            .SingleOrDefault<User>();
    }

    public User GetById(int id)
    {
        return _repo.GetById(id); 
    }

    public User GetByStripeId(string stripId)
    {
        if (stripId == null)
        {
            return null;
        }

        return _repo.Session.QueryOver<User>()
            .Where(u => u.StripeId == stripId)
            .SingleOrDefault();
    }

    public IList<User> GetWhereReputationIsBetween(int newReputation, int oldRepution)
    {
        var query = _repo.Session.QueryOver<User>();

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
        return _repo.Session
            .QueryOver<User>()
            .Where(u => u.GoogleId == googleId)
            .RowCount() == 1;
    }

    public User UserGetByFacebookId(string facebookId)
    {
        return _repo.Session
            .QueryOver<User>()
            .Where(u => u.FacebookId == facebookId)
            .SingleOrDefault();
    }

    public User UserGetByGoogleId(string googleId)
    {
        return _repo.Session
            .QueryOver<User>()
            .Where(u => u.GoogleId == googleId)
            .SingleOrDefault();
    }
}