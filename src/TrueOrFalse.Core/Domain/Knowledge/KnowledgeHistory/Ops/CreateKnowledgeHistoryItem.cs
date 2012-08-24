using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace TrueOrFalse.Core
{
    public class CreateKnowledgeHistoryItem
    {
        private readonly ISession _session;

        public CreateKnowledgeHistoryItem(ISession session){
            _session = session;
        }

        public void Run(int userId)
        {
            _session.CreateQuery("SELECT COUNT(*) FROM knowledgeitem WHERE UserId = " + userId +
                                 " AND CorrectnessProbability = -1");
        }
    }
}
