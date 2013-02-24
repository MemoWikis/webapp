using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse
{
    public class GetTotalQuestionSetCount : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;

        public GetTotalQuestionSetCount(ISession session){
            _session = session;
        }

        public int Run(){
            return (int)_session.CreateQuery("SELECT Count(Id) FROM QuestionSet").UniqueResult<Int64>();
        }
    }
}
