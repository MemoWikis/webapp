using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Infrastructure.Persistence
{
    /// <summary>
    /// Überwacht den Lifecycle einer NHibernate Session, damit
    /// Daten vor dem Schließen gespeichert werden.
    /// </summary>
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
}

