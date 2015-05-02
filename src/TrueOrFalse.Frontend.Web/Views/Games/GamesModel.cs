using System;
using System.Collections.Generic;
using System.Linq;

public class GamesModel : BaseModel
{
    public List<GameModelRow> Games;

    public GamesModel(IList<Game> games)
    {
        Games = games.Select(g => new GameModelRow(g)).ToList();
    }
}

public class GameModelRow
{
    public string GameId;
    public User Creator;
    public IList<User> Players;
    public IList<Set> Sets;
    public GameStatus Status;
    public DateTime WillStartAt;

    public GameModelRow(Game game)
    {
        GameId = game.Id.ToString();
        Creator = game.Creator;
        Players = game.Players;
        Sets = game.Sets;
        Status = game.Status;
        WillStartAt = game.WillStartAt;
    }   
}