using System;
using System.Collections.Generic;
using System.Linq;

public class GameRowModel : BaseModel
{
    public int GameId;
    public User Creator;
    public IList<User> Players;
    public IList<Set> Sets;
    public GameStatus Status;
    public DateTime WillStartAt;
    public int Rounds;

    public bool IsCreator { get { return Creator.Id == base.UserId; } }
    public bool IsPlayer { get { return Players.Any(p => p.Id == base.UserId); } }
    public bool IsPlayerOrCreator { get { return IsPlayer || IsCreator; } }

    public bool InProgress()
    {
        return Status == GameStatus.InProgress;
    }

    public GameRowModel(Game game)
    {
        GameId = game.Id;
        Creator = game.Creator;
        Players = game.Players;
        Sets = game.Sets;
        Status = game.Status;
        WillStartAt = game.WillStartAt;
        Rounds = game.Rounds;
    }
}