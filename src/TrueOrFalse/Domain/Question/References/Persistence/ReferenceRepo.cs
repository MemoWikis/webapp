using System.Collections.Generic;
using NHibernate;
using Seedworks.Lib.Persistence;

public class ReferenceRepo : RepositoryDb<Reference>
{
    public ReferenceRepo(ISession session) : base(session)
    {
    }

    public IList<Question> GetQuestionsForCategory(int categoryId)
    {
        return _session
            .QueryOver<Reference>()
            .Where(x => x.Category.Id == categoryId)
            .Select(x => x.Question)
            .List<Question>();
    }

    public void DeleteForQuestion(int questionId)
    {
        Session.CreateSQLQuery("DELETE FROM reference WHERE Question_id = :questionId")
            .SetParameter("questionId", questionId).ExecuteUpdate();

    }
}