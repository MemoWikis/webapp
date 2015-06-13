using NHibernate;

public class SetMessageRead : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public SetMessageRead(ISession session){_session = session;}

    public void Run(int msgId)
    {
        _session
            .CreateSQLQuery("UPDATE Message SET IsRead = " + 1 + " WHERE Id = " + msgId)
            .ExecuteUpdate();
    }
}
