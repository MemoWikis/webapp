using System;
using System.Collections.Generic;
using System.Linq;
using Seedworks.Lib.Persistence;

public class Game : DomainEntity
{
    public virtual DateTime WillStartAt { get; set; }

    public virtual User Creator { get; set; }
    public virtual IList<User> Players { get; set; }

    public virtual int MaxPlayers { get; set; }

    public virtual int RoundCount { get; set; }
    public virtual IList<GameRound> Rounds { get; set; } 

    public virtual IList<Set> Sets { get; set; }
    public virtual GameStatus Status { get; set; }
    public virtual string Comment { get; set; }

    public virtual bool IsInProgess { get { return Status == GameStatus.InProgress; } }
    public virtual bool IsCompleted { get { return Status == GameStatus.Completed; } }
    public virtual bool IsNeverStarted { get { return Status == GameStatus.NeverStarted; } }
    public virtual bool IsReady { get { return Status == GameStatus.Ready; } }

    public virtual bool AddPlayer(User user)
    {
        if(Players == null)
            Players = new List<User>();

        if (Players.Any(u => u.Id == user.Id))
            return false;

        Players.Add(user);
        return true;
    }

    public Game()
    {
        Rounds = new List<GameRound>();
    }

    public virtual Game AddRound(GameRound round)
    {
        round.Status = GameRoundStatus.Open;
        round.DateCreated = DateTime.Now;
        round.DateModified = DateTime.Now;

        round.Number = Rounds.Count() + 1;

        Rounds.Add(round);

        return this;
    }

    public virtual Game NextRound()
    {
        if(Rounds.Count == 0)
            throw new Exception("Can not go to next round, game " + Id + " has no rounds.");

        if (Rounds.All(r => r.Status != GameRoundStatus.Current))
        {
            var nextOpenRound = GetNextOpenRund();
            nextOpenRound.Status = GameRoundStatus.Current;
            nextOpenRound.StartTime = DateTime.Now;
            return this;
        }

        var index = Rounds.IndexOf(x => x.Status == GameRoundStatus.Current);

        Rounds[index].Status = GameRoundStatus.Completed;
        Rounds[index].EndTime = DateTime.Now;

        if (index == Rounds.Count - 1)
            return this;
        
        Rounds[index + 1].Status = GameRoundStatus.Current;
        Rounds[index + 1].StartTime = DateTime.Now;

        return this;
    }

    private GameRound GetNextOpenRund()
    {
        return Rounds.FirstOrDefault(x => x.Status == GameRoundStatus.Open);
    }

    public virtual GameRound GetCurrentRound()
    {
        return Rounds.FirstOrDefault(x => x.Status == GameRoundStatus.Current);
    }

    public virtual bool IsLastRoundCompleted()
    {
        return Rounds.Last().Status == GameRoundStatus.Completed;
    }
}