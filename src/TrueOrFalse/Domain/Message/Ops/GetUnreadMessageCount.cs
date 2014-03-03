using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse
{
    public class GetUnreadMessageCount : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;

        public GetUnreadMessageCount(ISession session){
            _session = session;
        }

        public int Run(int receiverId)
        {
            return (int)_session
                .CreateQuery("SELECT Count(Id) FROM Message WHERE IsRead = 0 AND ReceiverId = " + receiverId)
                .UniqueResult<Int64>();
        }
    }
}
