using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

public class LearningSessionRepo : RepositoryDbBase<LearningSession>
{
    public LearningSessionRepo(ISession session): base(session){

    }

    public void UpdateForDeletedSet(int setId)
    {
        Session.CreateSQLQuery("UPDATE learningsession SET SetToLearn_id = NULL WHERE SetToLearn_id = :setId")
                .SetParameter("setId", setId)
                .ExecuteUpdate();
    }

    public LearningSession GetLastWishSessionIfUncompleted(User user)
    {
        var result = Session.QueryOver<LearningSession>()
            .Where(l => l.User == user)
            .And(l => l.IsWishSession)
            .OrderBy(l => l.DateModified).Desc
            .List()
            .FirstOrDefault();

        if (result != null && !result.IsCompleted)
            return result;

        return null;
    }

    public DateTime? GetDateOfLastWishSession(User user)
    {
        var result = Session.QueryOver<LearningSession>()
            .Where(l => l.User == user)
            .And(l => l.IsWishSession)
            .OrderBy(l => l.DateCreated).Desc
            .List()
            .FirstOrDefault();

        return result?.DateCreated;
    }

    public int GetNumberOfSessionsForSet(int setId)
    {
        return Session
            .Query<LearningSession>()
            .Count(x => (x.SetToLearn.Id == setId) || (x.SetsToLearnIdsString.Contains(setId.ToString())));
    }

}