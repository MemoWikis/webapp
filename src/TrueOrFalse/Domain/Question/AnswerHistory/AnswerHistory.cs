using System;
using Seedworks.Lib.Persistence;

public class AnswerHistory : IPersistable, WithDateCreated
{
    public virtual int Id { get; set; }
    public virtual int UserId { get; set; }
    public virtual int QuestionId { get; set; }
    public virtual AnswerCorrectness AnswerredCorrectly { get; set; }
    public virtual string AnswerText { get; set; }
    public virtual Round Round { get; set; }

    public virtual Player Player { get; set; }

    /// <summary>Duration</summary>
    public virtual int Milliseconds { get; set; }
    public virtual DateTime DateCreated { get; set; }

    public virtual Question GetQuestion()
    {
        return Sl.R<QuestionRepository>().GetById(QuestionId);
    }
}