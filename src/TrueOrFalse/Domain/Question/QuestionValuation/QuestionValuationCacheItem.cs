using System;
using System.Diagnostics;

[DebuggerDisplay("QuestionId={Question.Id}, IsInWuwi: {IsInWishKnowledge}")]
public class QuestionValuationCacheItem
{
    public int Id;

    public User User;
    public QuestionCacheItem Question;
    public DateTime DateCreated;

    public int CorrectnessProbability;
    public int CorrectnessProbabilityAnswerCount;

    public KnowledgeStatus KnowledgeStatus;

    public bool IsInWishKnowledge;

    public static QuestionValuationCacheItem ToCacheItem(QuestionValuation questionValuation)
    {
        var val = questionValuation.IsInWishKnowledge();
        return new QuestionValuationCacheItem()
        {
            CorrectnessProbability = questionValuation.CorrectnessProbability,
            CorrectnessProbabilityAnswerCount = questionValuation.CorrectnessProbabilityAnswerCount,
            DateCreated = questionValuation.DateCreated,
            Id = questionValuation.Id,
            IsInWishKnowledge = questionValuation.IsInWishKnowledge(),
            KnowledgeStatus = questionValuation.KnowledgeStatus,
            Question = QuestionCacheItem.ToCacheQuestion(questionValuation.Question),
            User = questionValuation.User
        };
    }
}
