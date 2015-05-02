using System.Collections.Generic;
using Microsoft.AspNet.SignalR.Hubs;
using NHibernate;

public class GameRepo : RepositoryDbBase<Game>
{
    public GameRepo(ISession session) : base(session)
    {
    }

    public IList<Game> GetAllActive()
    {
        return _session.QueryOver<Game>()
            .Where(g =>
                g.Status == GameStatus.InProgress ||
                g.Status == GameStatus.Started)
            .List<Game>();
    }
}

