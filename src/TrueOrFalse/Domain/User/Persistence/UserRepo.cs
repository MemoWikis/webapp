using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NHibernate;
using TrueOrFalse.Search;
using NHibernate.Linq;

public class UserRepo : RepositoryDbBase<User>
{
    private readonly SearchIndexUser _searchIndexUser;

    public UserRepo(ISession session, SearchIndexUser searchIndexUser): base(session){
        _searchIndexUser = searchIndexUser;
    }

    public User GetByEmail(string emailAddress) => 
        _session.QueryOver<User>()
            .Where(k => k.EmailAddress == emailAddress)
            .SingleOrDefault<User>();

    public User GetByName(string name) => 
        _session.QueryOver<User>()
            .Where(k => k.Name == name)
            .SingleOrDefault<User>();

    public User UserGetByFacebookId(string facebookId) => 
        _session
            .QueryOver<User>()
            .Where(u => u.FacebookId == facebookId)
            .SingleOrDefault();

    public User UserGetByGoogleId(string googleId) =>
        _session
            .QueryOver<User>()
            .Where(u => u.GoogleId == googleId)
            .SingleOrDefault();

    public bool FacebookUserExists(string facebookId) => 
        _session
            .QueryOver<User>()
            .Where(u => u.FacebookId == facebookId)
            .RowCount() == 1;

    public bool GoogleUserExists(string googleId) =>
        _session
            .QueryOver<User>()
            .Where(u => u.GoogleId == googleId)
            .RowCount() == 1;

    public void RemoveFollowerInfo(FollowerInfo followerInfo)
    {
        _session
            .CreateSQLQuery("DELETE FROM user_to_follower WHERE Id =" + followerInfo.Id)
            .ExecuteUpdate();
        _session.Flush();
        ReputationUpdate.ForUser(followerInfo.User);
    }

    public override void Update(User user)
    {
        this.Update(user, false);
    }

    public void Update(User user, bool runSolrUpdateAsync = false)
    {
        Logg.r().Information("user update {Id} {Email} {Stacktrace}", user.Id, user.EmailAddress, new StackTrace());
        _searchIndexUser.Update(user, runSolrUpdateAsync);
        base.Update(user);
    }

    public override void Create(User user)
    {
        Logg.r().Information("user create {Id} {Email} {Stacktrace}", user.Id, user.EmailAddress, new StackTrace());
        base.Create(user);
        _searchIndexUser.Update(user);
    }

    public override void Delete(int id)
    {
        var user = GetById(id);

        if (Sl.R<SessionUser>().IsLoggedInUserOrAdmin(user.Id))
            throw new InvalidAccessException();

        _searchIndexUser.Delete(user);
        base.Delete(id);
    }

    public void DeleteFromAllTables(int userId)
    {   // Delete
        //Session.CreateSQLQuery("DELETE FROM persistentlogin WHERE UserId = :userId").SetParameter("userId", userId).ExecuteUpdate();
        //Session.CreateSQLQuery("DELETE FROM membership WHERE User_Id = :userId").SetParameter("userId", userId).ExecuteUpdate();
        //Session.CreateSQLQuery("DELETE FROM appaccess WHERE User_Id = :userId").SetParameter("userId", userId).ExecuteUpdate();

        //Session.CreateSQLQuery("DELETE FROM activitypoints WHERE User_Id = :userId").SetParameter("userId", userId).ExecuteUpdate();
        //Session.CreateSQLQuery("DELETE FROM setValuation WHERE UserId = :userId").SetParameter("userId", userId).ExecuteUpdate();
        //Session.CreateSQLQuery("DELETE FROM setView WHERE User_Id = :userId").SetParameter("userId", userId).ExecuteUpdate();
        //Session.CreateSQLQuery("DELETE FROM messageemail WHERE User_Id = :userId").SetParameter("userId", userId).ExecuteUpdate();
        //Session.CreateSQLQuery("DELETE FROM questionvaluation WHERE UserId = :userId").SetParameter("userId", userId).ExecuteUpdate();
        //Session.CreateSQLQuery("DELETE FROM categoryvaluation WHERE UserId = :userId").SetParameter("userId", userId).ExecuteUpdate();
        //Session.CreateSQLQuery("DELETE FROM answer WHERE UserId = :userId").SetParameter("userId", userId).ExecuteUpdate();
        //Session.CreateSQLQuery("Update imagemetadata Set userid  = null Where userid =  :userId").SetParameter("userId", userId).ExecuteUpdate();
        //Session.CreateSQLQuery("Update comment Set Creator_id  = null Where Creator_id = :userId").SetParameter("userId", userId).ExecuteUpdate();
        //Session.CreateSQLQuery("DELETE FROM badge WHERE User_Id = :userId").SetParameter("userId", userId).ExecuteUpdate();
        //Session.CreateSQLQuery("Update questionview  Set UserId = null Where UserId = :userId").SetParameter("userId", userId).ExecuteUpdate();
        //Session.CreateSQLQuery("DELETE FROM answer WHERE UserId = :userId").SetParameter("userId", userId).ExecuteUpdate();
        //Session.CreateSQLQuery("delete, g.*, p.*, w.* From questionview q " +
        //                       "left join game_round g ON q.Round_id = g.Id " +
        //                       "left join game_player p ON q.Player_Id = p.Id " +
        //                       "left join widgetView w ON q.WidgetView_id = w.Id " +
        //                       "Where q.UserId = :userid; ")
        //    .SetParameter("userid", userId ).ExecuteUpdate();
        //Session.CreateSQLQuery("UPDATE categoryChange c " +
        //                       "JOIN user u ON u.id = c.author_id Set c.author_id = null " +
        //                       "WHERE u.id =  :userid;")
        //    .SetParameter("userid", userId).ExecuteUpdate();
        //Session.CreateSQLQuery("Update questionchange qc set qc.Author_id = null Where Author_id = :userid")
        //    .SetParameter("userid", userId).ExecuteUpdate();
        //Session.CreateSQLQuery(
        //        "DELETE uf.* From  user u LEFT JOIN user_to_follower uf ON u.id = uf.user_id Where u.id = :userid")
        //    .SetParameter("userid", userId).ExecuteUpdate();
        //Session.CreateSQLQuery("Update questionSet Set creator_id  = null Where creator_Id = :userid").SetParameter("userid", userId).ExecuteUpdate();
        //Session.CreateSQLQuery("Delete From question where creator_id = :userid and (visibility = 1 Or visibility = 2)").SetParameter("userid", userId).ExecuteUpdate();
        Session.CreateSQLQuery("Update question  Set Creator_Id = null Where Creator_Id = :userId AND Visibility = 0").SetParameter("userId", userId).ExecuteUpdate();


    }

    public User GetMemuchoUser() => GetById(Settings.MemuchoUserId);

    public IList<User> GetByIds(List<int> userIds) => GetByIds(userIds.ToArray());

    public override IList<User> GetByIds(params int[] userIds)
    {
        var users = base.GetByIds(userIds);

        if (userIds.Length != users.Count)
        {
            var missingUsersIds = userIds.Where(id => !users.Any(u => id == u.Id)).ToList();
            Logg.r().Error($"Following user ids from solr not found: {string.Join(",", missingUsersIds.OrderBy(id => id))}");

        }

        return userIds.Select(t => users.FirstOrDefault(u => u.Id == t)).Where(x => x != null).ToList();
    }

    public IList<User> GetWhereReputationIsBetween(int newReputation, int oldRepution)
    {
        var query = _session.QueryOver<User>();

        if(newReputation < oldRepution)
            query.Where(u => u.Reputation > newReputation && u.Reputation < oldRepution);
        else
            query.Where(u => u.Reputation < newReputation && u.Reputation > oldRepution);
       
        return query.List();
    }

    public int GetTotalActivityPoints()
    {
        return _session.Query<User>().Sum(u => u.ActivityPoints);
    }

    public void UpdateActivityPointsData()
    {
        var totalPointCount = 0;
        foreach (var activityPoints in Sl.ActivityPointsRepo.GetActivtyPointsByUser(Sl.CurrentUserId))
        {
            totalPointCount += activityPoints.Amount;
        }

        var userLevel = UserLevelCalculator.GetLevel(totalPointCount);

        var user = GetByIds(Sl.SessionUser.UserId).First();
        user.ActivityPoints = totalPointCount;
        user.ActivityLevel = userLevel;
        Update(user);
    }
}