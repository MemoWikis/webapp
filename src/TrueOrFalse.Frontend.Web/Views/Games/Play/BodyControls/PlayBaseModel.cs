using System;
using System.Collections.Generic;

public class PlayBaseModel : BaseModel
{
    public int GameId;

    public int RoundCount;

    public DateTime WillStartAt;

    public List<User> Players = new List<User>();

    public PlayBaseModel(Game game)
    {
        GameId = game.Id;

        Players.Add(game.Creator);
        Players.AddRange(game.Players);

        RoundCount = game.RoundCount;

        WillStartAt = game.WillStartAt;
    }
}