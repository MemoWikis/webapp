using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Tests;

public class ContextGame : IRegisterAsInstancePerLifetime
{
    private readonly GameRepo _gameRepo;
    private readonly ContextUser _contextUser = ContextUser.New();

    public List<Game> All = new List<Game>();

    public ContextGame(GameRepo gameRepo)
    {
        _gameRepo = gameRepo;
    }

    public static ContextGame New()
    {
        return BaseTest.Resolve<ContextGame>();
    }

    public ContextGame Add(
        GameStatus gameStatus = GameStatus.Ready, 
        int amountQuestions = 5,
        int amountPlayers = 2)
    {
        var setContext = ContextSet.New().AddSet("Set");

        for (var i = 0; i < amountQuestions; i++)
            setContext.AddQuestion("Question " + 1, "Solution " + 1);
            
        var set = setContext.Persist().All.First();

        All.Add(new Game
        {
            Sets = new List<Set> { set },
            Status = gameStatus,
            RoundCount = amountQuestions,
            Players = Players(amountPlayers)
        });

        Sl.Resolve<AddRoundsToGame>().Run(All.Last());  

        return this;
    }

    public IList<Player> Players(int amountPlayers)
    {
        for (var i = 0; i < amountPlayers; i++)
            _contextUser.Add("user_" + i);

        _contextUser.Persist();

        return _contextUser.All.Select((u,index) => new Player{
            User = u,
            IsCreator = index == 1
        }).ToList();
    }

    public ContextGame Persist()
    {
        foreach (var comment in All)
            _gameRepo.Create(comment);

        return this;
    }
}