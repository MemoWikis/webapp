using NHibernate;

public class QuestionFeatureRepo : RepositoryDbBase<QuestionFeature>
{
    public QuestionFeatureRepo(ISession session) : base(session)
    {
    }

    public void TruncateTables()
    {
        _session.CreateSQLQuery("truncate table questionfeature").ExecuteUpdate();
        _session.CreateSQLQuery("truncate table questionfeature_to_question").ExecuteUpdate();
    }
}