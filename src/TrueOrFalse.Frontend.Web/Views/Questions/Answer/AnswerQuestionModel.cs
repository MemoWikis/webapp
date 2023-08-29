using TrueOrFalse;
using TrueOrFalse.Domain.Question.QuestionValuation;

public class AnswerQuestionModel
{
    public int QuestionId;
    public QuestionCacheItem Question;
    public UserTinyModel Creator;


    public HistoryAndProbabilityModel HistoryAndProbability;
    public AnswerQuestionModel(QuestionCacheItem question,
        int sessionUserId,
        TotalsPersUserLoader totalsPersUserLoader,
        SessionUserCache sessionUserCache)
    {
        var valuationForUser = totalsPersUserLoader.Run(sessionUserId, question.Id);
        var questionValuationForUser = NotNull.Run(new QuestionValuationCache(sessionUserCache).GetByFromCache(question.Id, sessionUserId));

        HistoryAndProbability = new HistoryAndProbabilityModel
        {
            AnswerHistory = new AnswerHistoryModel(question, valuationForUser),
            CorrectnessProbability = new CorrectnessProbabilityModel(question, questionValuationForUser),
            QuestionValuation = questionValuationForUser
        };
    }
}
