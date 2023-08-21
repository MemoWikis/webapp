using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

[DebuggerDisplay("QuestionId={Question.Id}, IsInWuwi: {IsInWishKnowledge}")]
public class QuestionValuationCacheItem
{
    public int Id;

    public UserCacheItem User;
    public QuestionCacheItem Question;
    public DateTime DateCreated;

    public int CorrectnessProbability;
    public int CorrectnessProbabilityAnswerCount;

    public KnowledgeStatus KnowledgeStatus;

    public bool IsInWishKnowledge;

    public static QuestionValuationCacheItem ToCacheItem(QuestionValuation questionValuation,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
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
            Question = EntityCache.
                GetQuestionById(questionValuation.Question.Id, httpContextAccessor, webHostEnvironment),
            User = EntityCache.
                GetUserById(questionValuation.User.Id, httpContextAccessor, webHostEnvironment)
        };
    }
}
