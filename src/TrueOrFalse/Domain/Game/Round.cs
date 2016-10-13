using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Seedworks.Lib.Persistence;

public class Round : DomainEntity
{
    public virtual GameRoundStatus Status { get; set; }
    public virtual Set Set { get; set; }
    public virtual Question Question { get; set; }

    public virtual DateTime? StartTime { get; set; }
    public virtual DateTime? EndTime { get; set; }

    public virtual int Number { get; set; }

    public virtual Game Game { get; set; }

    public virtual IList<Answer> Answers { get; set; }

    /// <summary>Seconds</summary>
    public virtual int RoundLength => 20;

    public virtual bool IsExpired()
    {
        Debug.Assert(StartTime != null, "StartTime != null");
        return ((DateTime)StartTime).AddSeconds(RoundLength) < DateTime.Now;
    }

    public virtual int SecondsElapsed() => (int)(DateTime.Now - StartTime.Value).TotalSeconds;

    public virtual bool AllPlayersDidAnswer() => 
        Game.Players.Count == Answers.Count(answer => !answer.IsView());

    public virtual bool IsThreeSecondsBeforeEnd() => SecondsElapsed() - 3 >= RoundLength;

    public Round()
    {
        Answers = new List<Answer>();
    }

}

public enum GameRoundStatus
{
    Open = 1,
    Current = 2,
    Completed = 3
}