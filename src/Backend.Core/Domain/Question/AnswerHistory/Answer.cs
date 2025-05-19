public class Answer : IPersistable, WithDateCreated, IAnswered
{
    public virtual AnswerCorrectness AnswerredCorrectly { get; set; }
    public virtual string AnswerText { get; set; }

    public virtual int InteractionNumber { get; set; }

    public virtual bool Migrated { get; set; }

    /// <summary>Duration</summary>
    public virtual int MillisecondsSinceQuestionView { get; set; }

    public virtual Question Question { get; set; }

    public virtual Guid QuestionViewGuid { get; set; }

    public virtual string QuestionViewGuidString
    {
        get => QuestionViewGuid == Guid.Empty ? null : QuestionViewGuid.ToString();
        set
        {
            if (value == null)
            {
                QuestionViewGuid = Guid.Empty;
                return;
            }

            QuestionViewGuid = new Guid(value);
        }
    }

    public virtual int UserId { get; set; }

    public virtual bool AnsweredCorrectly()
    {
        return AnswerredCorrectly == AnswerCorrectness.True || AnswerredCorrectly == AnswerCorrectness.MarkedAsTrue;
    }

    public virtual double GetAnswerOffsetInMinutes()
    {
        return (DateTimeX.Now() - DateCreated).TotalMinutes;
    }

    public virtual int Id { get; set; }

    public virtual DateTime DateCreated { get; set; }

    public virtual bool IsView()
    {
        return AnswerredCorrectly == AnswerCorrectness.IsView;
    }
}

public interface IAnswered
{
    bool AnsweredCorrectly();
    double GetAnswerOffsetInMinutes();
}