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

        public int AmountCreatedSets(int creatorId)
        {
            return Sl.Resolve<ISession>()
                .QueryOver<Set>()
                .Select(Projections.RowCount())
                .Where(q => q.Creator.Id == creatorId)
                .FutureValue<int>().Value;            
        }

        public int AmountCreatedCategories(int creatorId)
        {
            return Sl.Resolve<ISession>()
                .QueryOver<Category>()
                .Select(Projections.RowCount())
                .Where(q => q.Creator.Id == creatorId)
                .FutureValue<int>().Value;
        }

    }
}
