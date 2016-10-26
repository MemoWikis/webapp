using System;
using System.Collections.Generic;

public class PlayBaseModel : BaseModel
{
    public int GameId;

    public int RoundCount;

    public DateTime WillStartAt;
    public int RemainingSeconds;

    public List<Player> Players = new List<Player>();

    public PlayBaseModel(Game game)
    {
        GameId = game.Id;

        Players.AddRange(game.Players);

        RoundCount = game.RoundCount;

        WillStartAt = game.WillStartAt;
        RemainingSeconds = game.RemainingSeconds();
    }
}