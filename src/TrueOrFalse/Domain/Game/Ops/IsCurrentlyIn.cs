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
        var gamesAsPlayer = _session.QueryOver<Game>()
            .Where(g =>
                g.Status == GameStatus.InProgress ||
                g.Status == GameStatus.Ready)
            .JoinQueryOver<User>(g => g.Players)
            .Where(u => u.Id == userId)
            .List<Game>();

        var gamesAsCreator = _session.QueryOver<Game>()
            .Where(g =>
                (g.Status == GameStatus.InProgress ||
                 g.Status == GameStatus.Ready) &&
                 g.Creator.Id == userId)
            .List<Game>(); 

        var allGames = gamesAsPlayer.Union(gamesAsCreator).ToList();

        if (!allGames.Any())
            return new IsCurrentlyInGameResult{Yes = false};

        return new IsCurrentlyInGameResult
        {
            Yes = true,
            Game = allGames.First(),
            IsCreator = allGames.First().Creator.Id == userId
        };
    }
}

public class IsCurrentlyInGameResult
{
    public bool Yes;
    public bool IsCreator;
    public Game Game;
}