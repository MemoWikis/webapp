using NHibernate;


public class SessionManager : ISessionManager
{
    public SessionManager(ISession session)
    {
        Session = session;
    }

    public ISession Session { get; set; }

    public void Dispose()
    {
        if (Session.IsOpen)
        {
            Session.Flush();
            Session.Close();
        }

        Session.Dispose();
    }
}