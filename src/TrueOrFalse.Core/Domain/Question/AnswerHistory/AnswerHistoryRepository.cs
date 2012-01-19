using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class AnswerHistoryRepository : RepositoryDb<AnswerHistory> 
    {
        public AnswerHistoryRepository(ISession session) : base(session)
        {
        }
    }
}
