using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Seedworks.Lib.Persistence;

public class MessageEmailRepo : RepositoryDb<MessageEmail>
{
    public MessageEmailRepo(ISession session) : base(session)
    {
    }

    public MessageEmail GetMostRecentForUserAndType(int userId, MessageEmailTypes messageEmailType)
    {
        return _session.QueryOver<MessageEmail>()
            .Where(x => x.User.Id == userId)
            .And(x => x.MessageEmailType == messageEmailType)
            .OrderBy(x => x.DateCreated).Desc
            .List<MessageEmail>()
            .FirstOrDefault();
    }
}