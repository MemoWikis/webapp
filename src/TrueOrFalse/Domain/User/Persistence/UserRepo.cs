using System.Collections.Generic;
using System.Linq;
using NHibernate;
using TrueOrFalse.Search;

public class UserRepo : RepositoryDbBase<User>
{
    private readonly SearchIndexUser _searchIndexUser;

    public UserRepo(ISession session, SearchIndexUser searchIndexUser): base(session){
        _searchIndexUser = searchIndexUser;
    }

    public User GetByEmail(string emailAddress)
    {
        return _session.QueryOver<User>()
            .Where(k => k.EmailAddress == emailAddress)
            .SingleOrDefault<User>();
    }

    public User GetByName(string name)
    {
        return _session.QueryOver<User>()
            .Where(k => k.Name == name)
            .SingleOrDefault<User>();
    }

    public bool FacebookUserExists(string facebookId)
    {
        return _session
            .QueryOver<User>()
            .Where(u => u.FacebookId == facebookId)
            .RowCount() == 1;
    }

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
        _searchIndexUser.Update(user, runSolrUpdateAsync);
        base.Update(user);
    }

    public override void Create(User user)
    {
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

    public override IList<User> GetByIds(params int[] userIds)
    {
        var questions = base.GetByIds(userIds);
        return userIds.Select(t => questions.First(q => q.Id == t)).ToList();
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
}