using System.Linq;
using NHibernate;

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

    public LearningSession GetByStepId(int stepId)
    {
        return _session.QueryOver<LearningSession>()
            .JoinQueryOver<LearningSessionStep>(l => l.Steps)
            .Where(s => s.Id == stepId)
            .SingleOrDefault<LearningSession>();
    }
}