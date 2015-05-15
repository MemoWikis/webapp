
using NHibernate;

public class IsCurrentlyInGame : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public IsCurrentlyInGame(ISession session)
    {
        _session = session;
    }

    public bool Yes(int gameId, int playerId)
    {
        var rowCount = _session.QueryOver<Game>()
            .Where(g =>
                g.Status == GameStatus.InProgress ||
                g.Status == GameStatus.Ready)
            .And(g => g.Id == gameId)
            .JoinQueryOver<User>(g => g.Players)
            .Where(u => u.Id == playerId)
            .RowCount();

        return rowCount >= 1;
    }
}