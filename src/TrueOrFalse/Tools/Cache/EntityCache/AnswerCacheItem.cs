using System.Diagnostics;

[DebuggerDisplay("Id={Id} Name={Text}")]
[Serializable]
public class AnswerCacheItem
{
    public virtual AnswerCorrectness AnswerCorrectness { get; set; }

    public virtual int QuestionId { get; set; }

    public virtual int UserId { get; set; }

    public virtual int Id { get; set; }

    public static AnswerCacheItem AnswerToAnswerCacheItem(Answer answer)
    {
        return new AnswerCacheItem
        {
            AnswerCorrectness = answer.AnswerredCorrectly,
            QuestionId = answer.Question.Id,
            UserId = answer.UserId,
            Id = answer.Id,
        };
    }

    public static List<AnswerCacheItem> AnswersToAnswerCacheItems(IEnumerable<Answer> answers)
    {
        return answers.Select(AnswerToAnswerCacheItem).ToList();
    }

    public static void AddAnswerToCache(ExtendedUserCache extendedUserCache, Answer answer)
    {
        var answerCacheItem = AnswerToAnswerCacheItem(answer);
        var userId = answerCacheItem.UserId;
        if (userId < 1)
        {
            var question = EntityCache.GetQuestion(answer.Question.Id);
            if (question != null)
            {
                question.AnswersByAnonymousUsers.Add(answerCacheItem);
                EntityCache.AddOrUpdate(question);
            }
        }
        else
        {
            var user = extendedUserCache.GetUser(userId);
            user.Answers.AddOrUpdate(
                answerCacheItem.QuestionId,
                new List<AnswerCacheItem> { answerCacheItem },
                (key, existingList) =>
                {
                    existingList.Add(answerCacheItem);
                    return existingList;
                });
        }
    }
}
