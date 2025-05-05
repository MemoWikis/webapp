using NHibernate;

public interface ISessionManager : IDisposable
{
    ISession Session { get; }
}