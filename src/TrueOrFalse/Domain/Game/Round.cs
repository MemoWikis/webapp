using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    public virtual IList<AnswerHistory> Answers { get; set; }

    /// <summary>Seconds</summary>
    public virtual int RoundLength { get { return 20; } }

    public virtual bool IsOverdue()
    {
        Debug.Assert(StartTime != null, "StartTime != null");
        return ((DateTime)StartTime).AddSeconds(RoundLength) < DateTime.Now;
    }

    public Round()
    {
        Answers = new List<AnswerHistory>();
    }
}

public enum GameRoundStatus
{
    Open = 1,
    Current = 2,
    Completed = 3
}