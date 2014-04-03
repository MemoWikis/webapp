using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse
{
    public class GetTotalUsers : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;

        public GetTotalUsers(ISession session){
            _session = session;
        }

        public int Run(){
            return (int)_session.CreateQuery("SELECT Count(Id) FROM User").UniqueResult<Int64>();
        }
    }
}
