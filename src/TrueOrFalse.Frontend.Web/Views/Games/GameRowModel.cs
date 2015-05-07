using System;
using System.Collections.Generic;

public class GameRowModel
{
    public string GameId;
    public User Creator;
    public IList<User> Players;
    public IList<Set> Sets;
    public GameStatus Status;
    public DateTime WillStartAt;

    public GameRowModel(Game game)
    {
        GameId = game.Id.ToString();
        Creator = game.Creator;
        Players = game.Players;
        Sets = game.Sets;
        Status = game.Status;
        WillStartAt = game.WillStartAt;
    }   
}