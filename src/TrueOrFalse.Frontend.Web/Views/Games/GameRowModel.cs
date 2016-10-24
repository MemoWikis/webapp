using System;
using System.Collections.Generic;
using System.Linq;

public class GameRowModel : BaseModel
{
    public int GameId;
    public Player Creator;
    public IList<Player> Players;
    public IList<Set> Sets;
    public GameStatus Status;
    public DateTime WillStartAt;
    public int RemainingSeconds;
    public int RoundCount;
    public int CurrentRound;

    public bool IsCreator => Creator.User.Id == base.UserId;
    public bool IsPlayer => Players.Any(p => p.User.Id == base.UserId) && !IsCreator;
    public bool IsPlayerOrCreator => IsPlayer || IsCreator;

    public bool InProgress() => Status == GameStatus.InProgress;

    public GameRowModel(Game game)
    {
        GameId = game.Id;
        Creator = game.Players.Creator();
        Players = game.Players;
        Sets = game.Sets;
        Status = game.Status;
        WillStartAt = game.WillStartAt;
        RoundCount = game.RoundCount;
        CurrentRound = game.GetCurrentRoundNumber();
        RemainingSeconds = game.RemainingSeconds();
    }
}