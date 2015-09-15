using System;
using NHibernate;

public class AnswerFeatureRepo : RepositoryDbBase<AnswerFeature>
{
    public AnswerFeatureRepo(ISession session) : base(session)
    {
    }

    public void InsertRelation(int featureId, int answerHistoryId)
    {
        _session.CreateSQLQuery(
            String.Format(@"INSERT INTO `answerfeature_to_answerhistory` 
                (`AnswerFeature_id`, `AnswerHistory_id`) 
              VALUES 
                ({0}, {1});", featureId, answerHistoryId)).ExecuteUpdate();
    }
}