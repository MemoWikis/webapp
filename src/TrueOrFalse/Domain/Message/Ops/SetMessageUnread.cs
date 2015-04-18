using NHibernate;

public class SetMessageUnread : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public SetMessageUnread(ISession session) { _session = session; }

    public void Run(int msgId)
    {
        _session
            .CreateSQLQuery("UPDATE Message SET IsRead = " + 0 + " WHERE Id = " + msgId)
            .ExecuteUpdate();
    }

}