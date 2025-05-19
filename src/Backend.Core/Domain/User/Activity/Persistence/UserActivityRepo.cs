using NHibernate;

public class UserActivityRepo(ISession _session) : RepositoryDb<UserActivity>(_session)
{
    public void DeleteForPage(int pageId)
    {
        Session.CreateSQLQuery("DELETE FROM useractivity WHERE Page_id = :pageId")
            .SetParameter("pageId", pageId)
            .ExecuteUpdate();
    }

    public void DeleteForQuestion(int questionId)
    {
        Session.CreateSQLQuery("DELETE FROM useractivity WHERE Question_id = :questionId")
            .SetParameter("questionId", questionId)
            .ExecuteUpdate();
    }

    public void DeleteForUser(int userConcernedId, int userNotFollowedAnymore)
    {
        //to be called when userConcerned unfollows userNotFollowedAnymore
        Session.CreateSQLQuery(
                "DELETE FROM useractivity WHERE UserConcerned_id = :userConcernedId AND UserCauser_id = :userNotFollowedAnymore")
            .SetParameter("userConcernedId", userConcernedId)
            .SetParameter("userNotFollowedAnymore", userNotFollowedAnymore)
            .ExecuteUpdate();
    }

    public IList<UserActivity> GetByUser(User user, int amount = 10)
    {
        return Query
            .Where(x => x.UserConcerned == user)
            .OrderBy(x => x.At).Desc
            .Take(amount)
            .List<UserActivity>();
    }
}