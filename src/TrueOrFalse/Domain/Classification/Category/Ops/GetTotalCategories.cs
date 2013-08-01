using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse
{
    public class GetTotalCategories : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;

        public GetTotalCategories(ISession session){
            _session = session;
        }

        public int Run(){
            return (int)_session.CreateQuery("SELECT Count(Id) FROM Category").UniqueResult<Int64>();
        }
    }
}
