using System;
using System.Collections.Generic;
using Seedworks.Lib.Persistence;

public class Answer : IPersistable, WithDateCreated, IAnswered
{
    public virtual int Id { get; set; }
    public virtual int UserId { get; set; }
    public virtual Question Question { get; set; }
    public virtual AnswerCorrectness AnswerredCorrectly { get; set; }
    public virtual string AnswerText { get; set; }
    public virtual Round Round { get; set; }

    public virtual Player Player { get; set; }

    public virtual LearningSession LearningSession { get; set; }
    public virtual Guid LearningSessionStepGuid { get; set; }

    /// <summary>Duration</summary>
    public virtual int Milliseconds { get; set; }
    public virtual DateTime DateCreated { get; set; }

    public virtual IList<AnswerFeature> Features { get; set; }

    public virtual User GetUser()
    {
        return Sl.R<UserRepo>().GetById(UserId);
    }

    public virtual bool AnsweredCorrectly()
    {
        return AnswerredCorrectly == AnswerCorrectness.True 
            || AnswerredCorrectly == AnswerCorrectness.MarkedAsTrue;
    }

    public virtual double GetAnswerOffsetInMinutes()
    {
        return (DateTimeX.Now() - DateCreated).TotalMinutes;
    }
}

public interface IAnswered
{
    bool AnsweredCorrectly();
    double GetAnswerOffsetInMinutes();
}