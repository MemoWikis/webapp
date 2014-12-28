using System;
using NHibernate;

namespace TrueOrFalse
{
    public class GetWishSetCount : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;

        public GetWishSetCount(ISession session){
            _session = session;
        }

        public int Run(int userId)
        {
            return (int)_session.CreateQuery(
                "SELECT count(distinct sv.Id) FROM SetValuation sv " +
                "WHERE UserId = " + userId +
                "AND RelevancePersonal > -1 ")
                .UniqueResult<Int64>();
        }
    }
}
