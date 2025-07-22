using System.Diagnostics;

public class AnswerQuestionModel
{
    public int QuestionId;
    public QuestionCacheItem Question;
    public UserTinyModel Creator;

    public HistoryAndProbabilityModel HistoryAndProbability;

    public AnswerQuestionModel(
        QuestionCacheItem question,
        SessionUser sessionUser,
        TotalsPerUserLoader totalsPerUserLoader,
        ExtendedUserCache extendedUserCache)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        Log.Information("AnswerQuestionDetailsTimer 1 - {0}", stopWatch.Elapsed.TotalMilliseconds);

        var valuationForUser = totalsPerUserLoader.Run(sessionUser, question.Id);

        Log.Information("AnswerQuestionDetailsTimer 2 - {0}", stopWatch.Elapsed.TotalMilliseconds);

        var questionValuationForUser =
            NotNull.Run(
                new QuestionValuationCache(extendedUserCache).GetByFromCache(question.Id,
                    sessionUser.UserId));
        Log.Information("AnswerQuestionDetailsTimer 3 - {0}", stopWatch.Elapsed.TotalMilliseconds);
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