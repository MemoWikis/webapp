using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class QuestionInSetRepo : RepositoryDb<QuestionInSet>
    {
        public QuestionInSetRepo(ISession session) : base(session){}
    }
}
