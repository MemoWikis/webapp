using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NHibernate;
using TrueOrFalse.Search;

public class UserRepo : RepositoryDbBase<User>
{
    private readonly SessionUser _sessionUser;
  
    private readonly ActivityPointsRepo _activityPointsRepo;

    public UserRepo(ISession session,
        SessionUser sessionUser,
        ActivityPointsRepo activityPointsRepo) : base(session)
    {
        _sessionUser = sessionUser;
        _activityPointsRepo = activityPointsRepo;
    }

    public void ApplyChangeAndUpdate(int userId, Action<User> change)
    {
        var user = GetById(userId);
        change(user);
        Update(user);
    }

    public override void Create(User user)
    {
        Logg.r().Information("user create {Id} {Email} {Stacktrace}", user.Id, user.EmailAddress, new StackTrace());
        base.Create(user);
        SessionUserCache.AddOrUpdate(user);
        EntityCache.AddOrUpdate(UserCacheItem.ToCacheUser(user));
        Task.Run(async () => await new MeiliSearchUsersDatabaseOperations().CreateAsync(user));
    }

    public override void Delete(int id)
    {
        var user = GetById(id);

        if (_sessionUser.IsLoggedInUserOrAdmin())
        {
            throw new InvalidAccessException();
        }

        base.Delete(id);
        SessionUserCache.Remove(user);
        EntityCache.RemoveUser(id);
        Task.Run(async () => await new MeiliSearchUsersDatabaseOperations().DeleteAsync(user));
    }

    public void DeleteFromAllTables(int userId)
    {
        var user = _session.Get<User>(userId);
        Task.Run(async () => await new MeiliSearchUsersDatabaseOperations().DeleteAsync(user));

        Session.CreateSQLQuery("DELETE FROM persistentlogin WHERE UserId = :userId").SetParameter("userId", userId)
            .ExecuteUpdate();
        Session.CreateSQLQuery("DELETE FROM activitypoints WHERE User_Id = :userId").SetParameter("userId", userId)
            .ExecuteUpdate();
        Session.CreateSQLQuery("DELETE FROM messageemail WHERE User_Id = :userId").SetParameter("userId", userId)
            .ExecuteUpdate();
        Session.CreateSQLQuery("Update questionValuation SET Userid = null WHERE UserId = :userId")
            .SetParameter("userId", userId).ExecuteUpdate();
        Session.CreateSQLQuery("Update categoryValuation SET Userid = null WHERE UserId = :userId")
            .SetParameter("userId", userId).ExecuteUpdate();
        Session.CreateSQLQuery("UPDATE learningSession SET User_Id = null WHERE User_id = :userId")
            .SetParameter("userId", userId).ExecuteUpdate();
        //Session.CreateSQLQuery("UPDATE date SET User_Id = null WHERE User_id = :userId").SetParameter("userId", userId)
        //    .ExecuteUpdate();
        Session.CreateSQLQuery("UPDATE category SET Creator_Id = null WHERE Creator_id = :userId")
            .SetParameter("userId", userId).ExecuteUpdate();
        Session.CreateSQLQuery("UPDATE categoryview SET User_Id = null WHERE User_id = :userId")
            .SetParameter("userId", userId).ExecuteUpdate();
        Session.CreateSQLQuery("DELETE FROM answer WHERE UserId = :userId").SetParameter("userId", userId)
            .ExecuteUpdate();
        Session.CreateSQLQuery("Update imagemetadata Set userid  = null Where userid =  :userId")
            .SetParameter("userId", userId).ExecuteUpdate();
        Session.CreateSQLQuery("Update comment Set Creator_id  = null Where Creator_id = :userId")
            .SetParameter("userId", userId).ExecuteUpdate();
        Session.CreateSQLQuery("DELETE FROM answer WHERE UserId = :userId").SetParameter("userId", userId)
            .ExecuteUpdate();
        Session.CreateSQLQuery("Update questionview  Set UserId = null Where UserId = :userId")
            .SetParameter("userId", userId).ExecuteUpdate();

        Session.CreateSQLQuery("UPDATE categoryChange c " +
                               "JOIN user u ON u.id = c.author_id Set c.author_id = null " +
                               "WHERE u.id =  :userid;")
            .SetParameter("userid", userId).ExecuteUpdate();

        Session.CreateSQLQuery("Update questionchange qc set qc.Author_id = null Where Author_id = :userid")
            .SetParameter("userid", userId).ExecuteUpdate();

        Session.CreateSQLQuery(
                "DELETE uf.* From  user u LEFT JOIN user_to_follower uf ON u.id = uf.user_id Where u.id = :userid")
            .SetParameter("userid", userId).ExecuteUpdate();

        Session.CreateSQLQuery(
                "DELETE uf.* From  user u LEFT JOIN user_to_follower uf ON u.id = uf.Follower_id Where u.id = :userid")
            .SetParameter("userid", userId).ExecuteUpdate();
        Session.CreateSQLQuery(
                "Delete ua.* From Useractivity ua  Join question q ON ua.question_id = q.id where q.creator_id = :userid and (visibility = 1 Or visibility = 2)")
            .SetParameter("userid", userId).ExecuteUpdate();
        Session.CreateSQLQuery("Delete From question where creator_id = :userid and visibility = 1")
            .SetParameter("userid", userId).ExecuteUpdate();
        Session.CreateSQLQuery("Update question  Set Creator_Id = null Where Creator_Id = :userId")
            .SetParameter("userId", userId)
            .ExecuteUpdate(); // visibility not necessary because everything has already been deleted
        //Session.CreateSQLQuery(
        //        "Delete u.*, g.* From useractivity u Left Join  game g ON g.Id = u.Game_id Where u.UserCauser_id =  :userId;")
        //    .SetParameter("userId", userId).ExecuteUpdate();

        Session.CreateSQLQuery(
                "Delete ua.* From useractivity ua Left Join  user u ON u.id = ua.UserConcerned_id Where u.id  =  :userId;")
            .SetParameter("userId", userId).ExecuteUpdate();

        Session.CreateSQLQuery(
                "Delete ua.* From useractivity ua Left Join  user u ON u.id = ua.UserISFollowed_id Where u.id  =  :userId;")
            .SetParameter("userId", userId).ExecuteUpdate();

        Session.CreateSQLQuery("Delete From user Where id =  :userId;").SetParameter("userId", userId).ExecuteUpdate();
    }

    public bool FacebookUserExists(string facebookId)
    {
        return _session
            .QueryOver<User>()
            .Where(u => u.FacebookId == facebookId)
            .RowCount() == 1;
    }

    public User GetByEmail(string emailAddress)
    {
        return _session.QueryOver<User>()
            .Where(k => k.EmailAddress == emailAddress)
            .SingleOrDefault<User>();
    }

    public User GetByEmailEager(string email)
    {
        var user = _session
            .QueryOver<User>()
            .Where(u => u.EmailAddress == email)
            .SingleOrDefault();

        _session
            .QueryOver<User>()
            .Where(u => u.EmailAddress == email)
            .Fetch(SelectMode.Fetch, u => u.Followers)
            .SingleOrDefault();

        _session
            .QueryOver<User>()
            .Where(u => u.EmailAddress == email)
            .Fetch(SelectMode.Fetch, u => u.Following)
            .SingleOrDefault();
        return user;
    }

    public override IList<User> GetByIds(params int[] userIds)
    {
        var users = base.GetByIds(userIds);

        if (userIds.Length != users.Count)
        {
            var missingUsersIds = userIds.Where(id => !users.Any(u => id == u.Id)).ToList();
            Logg.r().Error(
                $"Following user ids from meilisearch not found: {string.Join(",", missingUsersIds.OrderBy(id => id))}");
        }

        return userIds.Select(t => users.FirstOrDefault(u => u.Id == t)).Where(x => x != null).ToList();
    }

    public User GetByName(string name)
    {
        return _session.QueryOver<User>()
            .Where(k => k.Name == name)
            .SingleOrDefault<User>();
    }

    public User GetByStripeId(string stripId)
    {
        if (stripId == null)
        {
            return null;
        }

        return _session.QueryOver<User>()
            .Where(u => u.StripeId == stripId)
            .SingleOrDefault();
    }

    public User GetMemuchoUser()
    {
        return GetById(Settings.MemuchoUserId);
    }

    public IList<User> GetWhereReputationIsBetween(int newReputation, int oldRepution)
    {
        var query = _session.QueryOver<User>();

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
        return _session
            .QueryOver<User>()
            .Where(u => u.GoogleId == googleId)
            .RowCount() == 1;
    }

    public override void Update(User user)
    {
        Logg.r().Information("user update {Id} {Email} {Stacktrace}", user.Id, user.EmailAddress, new StackTrace());
        base.Update(user);
        SessionUserCache.AddOrUpdate(user);
        EntityCache.AddOrUpdate(UserCacheItem.ToCacheUser(user));
        Task.Run(async () => await new MeiliSearchUsersDatabaseOperations().UpdateAsync(user));
    }

    public void Update(UserCacheItem userCacheItem)
    {
        var user = GetById(userCacheItem.Id);

        user.EmailAddress = userCacheItem.EmailAddress;
        user.Name = userCacheItem.Name;
        user.FacebookId = userCacheItem.FacebookId;
        user.GoogleId = userCacheItem.GoogleId;
        user.Reputation = userCacheItem.Reputation;
        user.ReputationPos = userCacheItem.ReputationPos;
        user.FollowerCount = userCacheItem.FollowerCount;
        user.ShowWishKnowledge = userCacheItem.ShowWishKnowledge;

        Update(user);
    }

    public void UpdateActivityPointsData()
    {
        if (!_sessionUser.IsLoggedIn)
        {
            return;
        }

        var totalPointCount = 0;
        foreach (var activityPoints in _activityPointsRepo.GetActivtyPointsByUser(_sessionUser.UserId))
        {
            totalPointCount += activityPoints.Amount;
        }

        var userLevel = UserLevelCalculator.GetLevel(totalPointCount);

        var user = GetById(_sessionUser.UserId);
        user.ActivityPoints = totalPointCount;
        user.ActivityLevel = userLevel;
        Update(user);
    }

    public User UserGetByFacebookId(string facebookId)
    {
        return _session
            .QueryOver<User>()
            .Where(u => u.FacebookId == facebookId)
            .SingleOrDefault();
    }

    public User UserGetByGoogleId(string googleId)
    {
        return _session
            .QueryOver<User>()
            .Where(u => u.GoogleId == googleId)
            .SingleOrDefault();
    }
}