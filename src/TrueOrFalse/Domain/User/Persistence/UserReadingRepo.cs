using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NHibernate;
using Seedworks.Lib.Persistence;
using ISession = NHibernate.ISession;


public class UserReadingRepo : RepositoryDb<User>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
   
    

    public UserReadingRepo(ISession session,
        IHttpContextAccessor httpContextAccessor, 
        IWebHostEnvironment webHostEnvironment) : base(session)

    {
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
      
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

    public User GetByEmailEager(string email)
    {
        var user = Session
            .QueryOver<User>()
            .Where(u => u.EmailAddress == email)
            .SingleOrDefault();

        Session
            .QueryOver<User>()
            .Where(u => u.EmailAddress == email)
            .Fetch(SelectMode.Fetch, u => u.Followers)
            .SingleOrDefault();

        Session
            .QueryOver<User>()
            .Where(u => u.EmailAddress == email)
            .Fetch(SelectMode.Fetch, u => u.Following)
            .SingleOrDefault();
        return user;
    }

    public IList<User> GetByIds(params int[] userIds)
    {
        var users = base.GetByIds(userIds);

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
        return Session.QueryOver<User>()
            .Where(k => k.Name == name)
            .SingleOrDefault<User>();
    }

    public User GetById(int id)
    {
        return base.GetById(id); 
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

    public User UserGetByFacebookId(string facebookId)
    {
        return Session
            .QueryOver<User>()
            .Where(u => u.FacebookId == facebookId)
            .SingleOrDefault();
    }

    public User UserGetByGoogleId(string googleId)
    {
        return Session
            .QueryOver<User>()
            .Where(u => u.GoogleId == googleId)
            .SingleOrDefault();
    }
}