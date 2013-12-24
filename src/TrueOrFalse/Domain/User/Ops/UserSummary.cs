using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;

namespace TrueOrFalse
{
    public class UserSummary : IRegisterAsInstancePerLifetime
    {
        public int AmountCreatedQuestions(int creatorId)
        {
            return Sl.Resolve<ISession>()
                .QueryOver<Question>()
                .Select(Projections.RowCount())
                .Where(q => q.Creator.Id == creatorId)
                .FutureValue<int>().Value;
        }
    }
}
