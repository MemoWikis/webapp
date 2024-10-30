using System.Diagnostics;

[DebuggerDisplay("Id={Id} Name={Text}")]
[Serializable]
public class AnswerCacheItem
{
    public virtual AnswerCorrectness AnswerredCorrectly { get; set; }
    public virtual string AnswerText { get; set; }

    public virtual int InteractionNumber { get; set; }

    /// <summary>Duration</summary>
    public virtual int MillisecondsSinceQuestionView { get; set; }

    public virtual int QuestionId { get; set; }

    public virtual Guid QuestionViewGuid { get; set; }

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

    public static AnswerCacheItem AnswerToAnswerCacheItem(Answer answer)
    {
        return new AnswerCacheItem
        {
            AnswerredCorrectly = answer.AnswerredCorrectly,
            AnswerText = answer.AnswerText,
            InteractionNumber = answer.InteractionNumber,
            MillisecondsSinceQuestionView = answer.MillisecondsSinceQuestionView,
            QuestionId = answer.Question.Id,
            QuestionViewGuid = answer.QuestionViewGuid,
            UserId = answer.UserId,
            Id = answer.Id,
            DateCreated = answer.DateCreated
        };
    }

    public static List<AnswerCacheItem> AnswersToAnswerCacheItems(IEnumerable<Answer> answers)
    {
        return answers.Select(AnswerToAnswerCacheItem).ToList();
    }
}
