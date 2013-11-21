using System;
using NHibernate;
using NHibernate.Criterion;

namespace TrueOrFalse
{
    public class GetTotalQuestionCount : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;

        public GetTotalQuestionCount(ISession session){
            _session = session;
        }

        public int Run(){
            return (int)_session.CreateQuery("SELECT Count(Id) FROM Question").UniqueResult<Int64>();
        }

        public int Run(int creatorId)
        {
            return _session.QueryOver<Question>()
                .Where(s => s.Creator.Id == creatorId)
                .Select(Projections.RowCount())
                .FutureValue<int>()
                .Value;
        }
    }
}