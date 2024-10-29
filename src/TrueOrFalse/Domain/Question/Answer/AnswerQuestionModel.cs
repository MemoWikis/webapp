using System.Diagnostics;
using TrueOrFalse;
using TrueOrFalse.Domain.Question.QuestionValuation;

public class AnswerQuestionModel
{
    public int QuestionId;
    public QuestionCacheItem Question;
    public UserTinyModel Creator;

    public HistoryAndProbabilityModel HistoryAndProbability;

    public AnswerQuestionModel(
        QuestionCacheItem question,
        int sessionUserId,
        TotalsPerUserLoader totalsPerUserLoader,
        ExtendedUserCache extendedUserCache)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        Logg.r.Information("AnswerQuestionDetailsTimer 1 - {0}", stopWatch.ElapsedMilliseconds);

        var valuationForUser = totalsPerUserLoader.Run(sessionUserId, question.Id);

        Logg.r.Information("AnswerQuestionDetailsTimer 2 - {0}", stopWatch.ElapsedMilliseconds);

        var questionValuationForUser =
            NotNull.Run(
                new QuestionValuationCache(extendedUserCache).GetByFromCache(question.Id,
                    sessionUserId));
        Logg.r.Information("AnswerQuestionDetailsTimer 3 - {0}", stopWatch.ElapsedMilliseconds);
        stopWatch.Stop();

        HistoryAndProbability = new HistoryAndProbabilityModel
        {
            AnswerHistory = new AnswerHistoryModel(question, valuationForUser),
            CorrectnessProbability =
                new CorrectnessProbabilityModel(question, questionValuationForUser),
            QuestionValuation = questionValuationForUser
        };
    }
}

public record struct HistoryAndProbabilityModel(
    AnswerHistoryModel AnswerHistory,
    CorrectnessProbabilityModel CorrectnessProbability,
    QuestionValuationCacheItem QuestionValuation);