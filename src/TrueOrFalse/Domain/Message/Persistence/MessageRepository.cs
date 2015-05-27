using System.Collections.Generic;
using NHibernate;
using Seedworks.Lib.Persistence;

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