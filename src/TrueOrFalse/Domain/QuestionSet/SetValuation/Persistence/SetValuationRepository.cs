using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Transform;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class SetValuationRepository : RepositoryDb<SetValuation> 
    {
        public SetValuationRepository(ISession session) : base(session){}

        public SetValuation GetBy(int questionId, int userId)
        {
            return _session.QueryOver<SetValuation>()
                           .Where(q => q.UserId == userId && q.SetId == questionId)
                           .SingleOrDefault();
        }

        public IList<SetValuation> GetBy(IList<int> setIds, int userId)
        {
            if (!setIds.Any())
                return new List<SetValuation>();

            var sb = new StringBuilder();
            sb.Append("SELECT * FROM SetValuation WHERE UserId = " + userId + " ");
            sb.Append("AND (SetId = " + setIds[0]);

            for(int i = 1; i < setIds.Count(); i++){
                sb.Append(" OR SetId = " + setIds[i]);
            }
            sb.Append(")");

            Console.Write(sb.ToString());

            return _session.CreateSQLQuery(sb.ToString())
                           .SetResultTransformer(Transformers.AliasToBean(typeof(SetValuation)))
                           .List<SetValuation>();
        }
    }
}