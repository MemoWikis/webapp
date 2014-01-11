using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Utils;
using NHibernate;
using NHibernate.Criterion;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
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

        public IList<AnswerHistory> GetBy(List<int> questionsId, int userId)
        {
            return Session.QueryOver<AnswerHistory>()
                .Where(Restrictions.In("QuestionId",questionsId))
                .List();
        }

        public IList<AnswerHistory> GetBy(int questionId, int userId)
        {
            return Session.QueryOver<AnswerHistory>()
                          .Where(i => i.QuestionId == questionId && i.UserId == userId)
                          .List<AnswerHistory>();
        }
    }
}
