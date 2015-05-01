using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Utils;
using TrueOrFalse.Tests;

public class ContextGame : IRegisterAsInstancePerLifetime
{
    private readonly GameRepo _gameRepo;

    private readonly ContextUser _contextUser = ContextUser.New();
    private readonly ContextSet _contextSet = ContextSet.New();

    public List<Game> All = new List<Game>();

    private readonly User _user1;
    private readonly Set _set1;

    public ContextGame(GameRepo gameRepo)
    {
        _user1 = _contextUser.Add("Test").Persist().All.First();
        _set1 = _contextSet.AddSet("Set").Persist().All.First();

        _gameRepo = gameRepo;
    }

    public static ContextGame New()
    {
        return BaseTest.Resolve<ContextGame>();
    }

    public ContextGame Add()
    {
        All.Add(new Game
        {
            Creator = _user1,
            Sets = new List<Set> { _set1 }
        });

        return this;
    }

    public ContextGame Persist()
    {
        foreach (var comment in All)
            _gameRepo.Create(comment);

        return this;
    }
}