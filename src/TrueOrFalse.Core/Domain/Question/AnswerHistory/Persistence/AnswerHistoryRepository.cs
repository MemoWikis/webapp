using System.Collections.Generic;
using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class AnswerHistoryRepository : RepositoryDb<AnswerHistory> 
    {
        public ISession Session { get { return _session; } }

        public AnswerHistoryRepository(ISession session) : base(session){}

        public void DeleteFor(int questionId)
        {
            Session.CreateSQLQuery("DELETE FROM AnswerHistory ah WHERE ah.QuestionId = :questionId")
                   .SetParameter("questionId", questionId);
        }

        public IList<AnswerHistory> GetBy(int questionId, int userId)
        {
            return Session.QueryOver<AnswerHistory>()
                          .Where(i => i.QuestionId == questionId && i.UserId == userId)
                          .List<AnswerHistory>();
        }
    }
}
