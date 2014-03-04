using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class MessageRepository : RepositoryDb<Message>
    {
        public MessageRepository(ISession session) : base(session)
        {
        }

        public IList<Message> GetForUser(int userId)
        {
            return _session.QueryOver<Message>()
                .Where(x => x.ReceiverId == userId)
                .OrderBy(x => x.DateCreated).Desc
                .List<Message>();
        }
    }
}
