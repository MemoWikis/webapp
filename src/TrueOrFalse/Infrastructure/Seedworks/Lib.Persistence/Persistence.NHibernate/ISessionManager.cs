using NHibernate;

namespace Seedworks.Lib.Persistence
{
    public interface ISessionManager : IDisposable
    {
        ISession Session { get; }
    }
}

