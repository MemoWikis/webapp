using Microsoft.AspNet.SignalR.Hubs;
using NHibernate;

public class GameRepo : RepositoryDbBase<Game>
{
    public GameRepo(ISession session) : base(session)
    {
    }
}

