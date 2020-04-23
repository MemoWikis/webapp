using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;
using Seedworks.Lib.Persistence;

public class Game : DomainEntity
{
    public virtual DateTime WillStartAt { get; set; }
    public virtual int RemainingSeconds() => (int) (WillStartAt - DateTime.Now).TotalSeconds;

    public virtual IList<Player> Players { get; set; }

    public virtual int MaxPlayers { get; set; }

    public virtual int RoundCount { get; set; }
    public virtual IList<Round> Rounds { get; set; } 

    public virtual IList<Set> Sets { get; set; }
    public virtual GameStatus Status { get; set; }
    public virtual string Comment { get; set; }

    public virtual bool IsInProgess => Status == GameStatus.InProgress;
    public virtual bool IsCompleted => Status == GameStatus.Completed;
    public virtual bool IsNeverStarted => Status == GameStatus.NeverStarted;
    public virtual bool IsReady => Status == GameStatus.Ready;

    public virtual bool WithSystemAvgPlayer { get; set; }

    public virtual bool AddPlayer(User user, bool isCreator = false)
    {
        if(Players == null)
            Players = new List<Player>();

        if (Players.Any(player => player.User.Id == user.Id))
            return false;

        Players.Add(new Player
        {
            Game = this,
            User = user,
            IsCreator = isCreator,
            DateModified = DateTime.Now,
            DateCreated = DateTime.Now,
        });
        return true;
    }

    public virtual void RemovePlayer(int playerUserId)
    {
        if (Players == null)
            return;

        if (Players.All(p => p.User.Id != playerUserId))
            return;

        Players.Remove(Players.First(p => p.User.Id == playerUserId));
    }

    public Game()
    {
        Rounds = new List<Round>();
    }

    public virtual Game AddRound(Round round)
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

    private Round GetNextOpenRund() => 
        Rounds.FirstOrDefault(x => x.Status == GameRoundStatus.Open);

    public virtual Round GetCurrentRound() => 
        Rounds.FirstOrDefault(x => x.Status == GameRoundStatus.Current);

    public virtual int GetCurrentRoundNumber()
    {
        int CurrentRoundNum = -1;
        var currentRound = GetCurrentRound();
        if (currentRound != null)
            CurrentRoundNum = currentRound.Number;
        else
        {
            if (IsCompleted)
                CurrentRoundNum = RoundCount;
            else if (IsReady)
                CurrentRoundNum = 1;
        }

        return CurrentRoundNum;
    }

    public virtual bool IsLastRoundCompleted()
    {
        return Rounds.Last().Status == GameRoundStatus.Completed;
    }

    public virtual void SetPlayerPositions()
    {
        int position = 0;

        Players
            .OrderByDescending(p => p.AnsweredCorrectly)
            .ThenBy(p => p.AnsweredWrong)
            .GroupBy(p => p.AnsweredCorrectly)
            .ForEach(groupLevel1 =>
            {   
                groupLevel1
                    .GroupBy(p => p.AnsweredWrong)
                    .ForEach(groupLevel2 =>
                    {
                        position++;
                        groupLevel2.ForEach(player => {player.Position = position;});
                    });
            });

        Players = Players.OrderBy(player => player.Position).ToList();
    }

    public virtual Player GetWinner()
    {
        SetPlayerPositions();
        return Players.First();
    }
}