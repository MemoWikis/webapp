using System.Linq;
using NHibernate;

public class IsCurrentlyIn : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public IsCurrentlyIn(ISession session)
    {
        _session = session;
    }

    public IsCurrentlyInGameResult Game(int userId)
    {
        var games = _session.QueryOver<Game>()
            .Where(g =>
                g.Status == GameStatus.InProgress ||
                g.Status == GameStatus.Ready)
            .JoinQueryOver<Player>(g => g.Players)
            .Where(p => p.User.Id == userId)
            .List<Game>();

        if (!games.Any())
            return new IsCurrentlyInGameResult{Yes = false};

        return new IsCurrentlyInGameResult
        {
            Yes = true,
            Game = games.First(),
            IsCreator = games.First().Players.Creator().Id == userId
        };
    }
}

public class IsCurrentlyInGameResult
{
    public bool Yes;
    public bool IsCreator;
    public Game Game;
}