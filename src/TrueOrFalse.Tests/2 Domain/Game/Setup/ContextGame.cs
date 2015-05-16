using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Tests;

public class ContextGame : IRegisterAsInstancePerLifetime
{
    private readonly GameRepo _gameRepo;

    private readonly ContextUser _contextUser = ContextUser.New();

    public List<Game> All = new List<Game>();

    private readonly User _user1;

    public ContextGame(GameRepo gameRepo)
    {
        _user1 = _contextUser.Add("Test").Persist().All.First();

        _gameRepo = gameRepo;
    }

    public static ContextGame New()
    {
        return BaseTest.Resolve<ContextGame>();
    }

    public ContextGame Add(
        GameStatus gameStatus = GameStatus.Ready, 
        int amountQuestions = 5)
    {
        var setContext = ContextSet.New().AddSet("Set");

        for (var i = 0; i < amountQuestions; i++)
            setContext.AddQuestion("Question " + 1, "Solution " + 1);
            
        var set = setContext.Persist().All.First();

        All.Add(new Game
        {
            Creator = _user1,
            Sets = new List<Set> { set },
            Status = gameStatus,
            RoundCount = amountQuestions
        });

        Sl.Resolve<AddRoundsToGame>().Run(All.Last());  

        return this;
    }

    public ContextGame Persist()
    {
        foreach (var comment in All)
            _gameRepo.Create(comment);

        return this;
    }
}