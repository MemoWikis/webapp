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

    public void DeleteForDate(int dateId)
    {
        Session.CreateSQLQuery("DELETE FROM useractivity WHERE Date_id = :dateId")
                .SetParameter("dateId", dateId)
                .ExecuteUpdate();
    }
}