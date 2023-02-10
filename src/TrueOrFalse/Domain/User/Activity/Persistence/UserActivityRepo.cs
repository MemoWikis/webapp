using System.Collections.Generic;
using NHibernate;
using Seedworks.Lib.Persistence;

public class UserActivityRepo : RepositoryDb<UserActivity> 
{
    public UserActivityRepo(ISession session) : base(session) { }

    public IList<UserActivity> GetByUser(User user, int amount = 10)
    {
        return Query
            .Where(x => x.UserConcerned == user)
            .OrderBy(x => x.At).Desc
            .Take(amount)
            .List<UserActivity>();
    }

    public void DeleteForQuestion(int questionId)
    {
        Session.CreateSQLQuery("DELETE FROM useractivity WHERE Question_id = :questionId")
                .SetParameter("questionId", questionId)
                .ExecuteUpdate();
    }

    public void DeleteForCategory(int categoryId)
    {
        Session.CreateSQLQuery("DELETE FROM useractivity WHERE Category_id = :categoryId")
                .SetParameter("categoryId", categoryId)
                .ExecuteUpdate();
    }

    public void DeleteForDate(int dateId)
    {
        Session.CreateSQLQuery("DELETE FROM useractivity WHERE Date_id = :dateId")
                .SetParameter("dateId", dateId)
                .ExecuteUpdate();
    }

    public void DeleteForSet(int setId)
    {
        Session.CreateSQLQuery("DELETE FROM useractivity WHERE Set_id = :setId")
                .SetParameter("setId", setId)
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
}