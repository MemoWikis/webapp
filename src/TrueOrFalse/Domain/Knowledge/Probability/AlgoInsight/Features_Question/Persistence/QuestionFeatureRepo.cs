using System;
using NHibernate;

public class QuestionFeatureRepo : RepositoryDbBase<QuestionFeature>
{
    public QuestionFeatureRepo(ISession session) : base(session)
    {
    }

    public void InsertRelation(int featureId, int questionId)
    {
        _session.CreateSQLQuery(
            String.Format(@"INSERT INTO `questionfeature_to_question` 
                (`QuestionFeature_id`, `Question_id`) 
              VALUES 
                ({0}, {1});", featureId, questionId)).ExecuteUpdate();
    }

    public void TruncateTables()
    {
        _session.CreateSQLQuery("truncate table questionfeature").ExecuteUpdate();
        _session.CreateSQLQuery("truncate table questionfeature_to_question").ExecuteUpdate();
    }
}