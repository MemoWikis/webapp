// Service for managing question counters in learning sessions
public class QuestionCounterService(
    SessionUser _sessionUser,
    ExtendedUserCache _extendedUserCache)
{
    public void Count(QuestionProperties questionProperties, QuestionCounter counter)
    {
        if (questionProperties.NotLearned) 
            counter.NotLearned++;

        if (questionProperties.NeedsLearning) 
            counter.NeedsLearning++;

        if (questionProperties.NeedsConsolidation) 
            counter.NeedsConsolidation++;

        if (questionProperties.Solid) 
            counter.Solid++;

        if (questionProperties.InWishKnowledge) 
            counter.InWishKnowledge++;

        if (questionProperties.NotInWishKnowledge) 
            counter.NotInWishKnowledge++;

        if (questionProperties.CreatedByCurrentUser) 
            counter.CreatedByCurrentUser++;

        if (questionProperties.NotCreatedByCurrentUser) 
            counter.NotCreatedByCurrentUser++;

        if (questionProperties.Public) 
            counter.Public++;

        if (questionProperties.Private) 
            counter.Private++;
    }

    public QuestionCounter CreateAnonymousUserCounter(int questionCount)
    {
        return new QuestionCounter
        {
            Max = questionCount,
            NotInWishKnowledge = questionCount,
            NotCreatedByCurrentUser = questionCount,
            NotLearned = questionCount,
            Public = questionCount
        };
    }

    public QuestionCounter BuildCounter(
        IList<QuestionCacheItem> allQuestions,
        LearningSessionConfig config,
        QuestionFilterService questionFilterService)
    {
        var questionCounter = new QuestionCounter();

        if (!_sessionUser.IsLoggedIn)
        {
            return CreateAnonymousUserCounter(allQuestions.Count);
        }

        var allQuestionValuations = _extendedUserCache.GetQuestionValuations(_sessionUser.UserId);
        var userQuestionValuations = _extendedUserCache.GetItem(_sessionUser.UserId)?.QuestionValuations;

        foreach (var question in allQuestions)
        {
            var questionProperties = questionFilterService.BuildQuestionProperties(
                question,
                config,
                allQuestionValuations,
                userQuestionValuations);

            if (questionProperties.AddToLearningSession)
            {
                questionCounter.Max++;
            }

            Count(questionProperties, questionCounter);
        }

        return questionCounter;
    }
}
