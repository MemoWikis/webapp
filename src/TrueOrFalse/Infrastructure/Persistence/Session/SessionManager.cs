using NHibernate;
using Seedworks.Lib.Persistence;
using System;

namespace TrueOrFalse.Infrastructure.Persistence
{
    public class SessionManager : ISessionManager, IDisposable
    {
        public ISession Session { get; private set; }

        public SessionManager(ISession session)
        {
            Session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Session.IsOpen)
                {
                    Session.Flush();
                    Session.Close();
                }

                Session.Dispose();
            }
        }
        //Destructor Forces unused sessions to be released even if they are not manually closed
        ~SessionManager()
        {
            Dispose(false);
        }
    }
}