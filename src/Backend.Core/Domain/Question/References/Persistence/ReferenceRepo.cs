using NHibernate;


public class ReferenceRepo : RepositoryDb<Reference>
{
    public ReferenceRepo(ISession session) : base(session)
    {
    }

    public void DeleteForQuestion(int questionId)
    {
            Session.CreateSQLQuery("DELETE FROM reference WHERE Question_id = :questionId")
                .SetParameter("questionId", questionId).ExecuteUpdate();
    }
}