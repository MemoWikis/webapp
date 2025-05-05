using NHibernate;


public class MessageRepo : RepositoryDb<Message>
{
    public MessageRepo(ISession session) : base(session)
    {
    }

    public IList<Message> GetForUser(int userId, bool onlyUnread = false)
    {
        if (onlyUnread)
        {
            return _session.QueryOver<Message>()
                .Where(x => x.ReceiverId == userId)
                .And(m => m.IsRead == false)
                .OrderBy(x => x.DateCreated).Desc
                .List<Message>();
        }

        return _session.QueryOver<Message>()
            .Where(x => x.ReceiverId == userId)
            .OrderBy(x => x.DateCreated).Desc
            .List<Message>();
    }

    public int GetNumberOfReadMessages(int userId)
    {
        return _session
            .Query<Message>()
            .Count(m => m.ReceiverId == userId && m.IsRead);
    }
}