using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;

public class GameRepo : RepositoryDbBase<Game>
{
    public GameRepo(ISession session) : base(session)
    {
    }

    public override void Create(Game game)
    {
        base.Create(game);
        Flush();
        UserActivityAdd.CreatedGame(game);
    }
    
    public IList<Game> GetAllActive()
    {
        return _session.QueryOver<Game>()
            .Where(g =>
                g.Status == GameStatus.InProgress ||
                g.Status == GameStatus.Ready)
            .List<Game>();
    }

    public IList<Game> GetOverdue()
    {
        return _session
            .QueryOver<Game>()
            .Where(g => g.Status == GameStatus.Ready)
            .And(g => g.WillStartAt < DateTime.Now.AddSeconds(3))
            .List<Game>();        
    }

    public IList<Game> AllCompletedByUser(int userId)
    {
        return _session
            .QueryOver<Game>()
            .Where(g => g.Status == GameStatus.Completed)
            .JoinQueryOver<Player>(g => g.Players)
            .Where(p => p.Id == userId)
            .List();
    }

    public IList<Game> GetRunningGames()
    {
        Round roundsAlias = null;
        Answer answerAlias = null;

        return _session
            .QueryOver<Game>()
            .JoinAlias(g => g.Rounds, () => roundsAlias)
            .JoinAlias(() => roundsAlias.Answers, () => answerAlias)
            .Where(g => g.Status == GameStatus.InProgress)
            .List<Game>();        
    }
}