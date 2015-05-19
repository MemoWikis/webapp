using System;
using System.Collections.Generic;

public class PlayBaseModel : BaseModel
{
    public int GameId;

    public int RoundCount;
    
    public int CurrentRoundNum;
    public GameRound CurrentRound;

    public DateTime WillStartAt;

    public List<User> Players = new List<User>();

    public PlayBaseModel(Game game)
    {
        GameId = game.Id;

        Players.Add(game.Creator);
        Players.AddRange(game.Players);

        RoundCount = game.RoundCount;

        CurrentRound = game.GetCurrentRound();
        if (CurrentRound != null)
            CurrentRoundNum = CurrentRound.Number;
        else
        {
            if (game.IsCompleted)
                CurrentRoundNum = game.Rounds.Count;

            CurrentRoundNum = 1;
        }

        WillStartAt = game.WillStartAt;
    }
}