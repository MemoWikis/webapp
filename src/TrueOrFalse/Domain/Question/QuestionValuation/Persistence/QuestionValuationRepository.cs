using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Transform;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class QuestionValuationRepository : RepositoryDb<QuestionValuation> 
    {
        public QuestionValuationRepository(ISession session) : base(session){}

        public QuestionValuation GetBy(int questionId, int userId)
        {
            return _session.QueryOver<QuestionValuation>()
                           .Where(q => q.UserId == userId && q.QuestionId == questionId)
                           .SingleOrDefault();
        }

        public IList<QuestionValuation> GetBy(IList<int> questionIds, int userId)
        {
            if(!questionIds.Any())
                return new List<QuestionValuation>();

            var sb = new StringBuilder();
            sb.Append("SELECT * FROM QuestionValuation WHERE UserId = " + userId + " ");
            sb.Append("AND (QuestionId = " + questionIds[0]);

            for(int i = 1; i < questionIds.Count(); i++){
                sb.Append(" OR QuestionId = " + questionIds[i]);
            }
            sb.Append(")");

            Console.Write(sb.ToString());

            return _session.CreateSQLQuery(sb.ToString())
                           .SetResultTransformer(Transformers.AliasToBean(typeof(QuestionValuation)))
                           .List<QuestionValuation>();
        }
    }
}
