// Service for sorting and organizing questions in learning sessions
public class QuestionSortingService(SessionUser _sessionUser)
{
    public IList<QuestionCacheItem> SortQuestions(
        IList<QuestionCacheItem> questions,
        LearningSessionConfig config,
        IList<KnowledgeSummaryDetail> knowledgeSummaryDetails)
    {
        return config.QuestionOrder switch
        {
            QuestionOrder.SortByEasiest => SortByEasiest(questions),
            QuestionOrder.SortByHardest => SortByHardest(questions),
            QuestionOrder.SortByPersonalHardest when _sessionUser.IsLoggedIn => SortByPersonalHardest(questions, knowledgeSummaryDetails),
            _ => questions
        };
    }

    public IList<QuestionCacheItem> LimitQuestionCount(
        IList<QuestionCacheItem> questions,
        LearningSessionConfig config)
    {
        if (config.MaxQuestionCount > questions.Count ||
            config.MaxQuestionCount == -1 ||
            config.MaxQuestionCount == 0)
        {
            return questions;
        }

        return questions.Take(config.MaxQuestionCount).ToList();
    }

    public IList<QuestionCacheItem> ShuffleQuestions(IList<QuestionCacheItem> questions)
    {
        return questions.Shuffle();
    }

    private static IList<QuestionCacheItem> SortByEasiest(IList<QuestionCacheItem> questions)
    {
        return questions.OrderByDescending(q => q.CorrectnessProbability).ToList();
    }

    private static IList<QuestionCacheItem> SortByHardest(IList<QuestionCacheItem> questions)
    {
        return questions.OrderBy(q => q.CorrectnessProbability).ToList();
    }

    private static IList<QuestionCacheItem> SortByPersonalHardest(
        IList<QuestionCacheItem> questions,
        IList<KnowledgeSummaryDetail> knowledgeSummaryDetails)
    {
        var orderedKnowledgeSummaryDetails = knowledgeSummaryDetails
            .OrderBy(k => k.PersonalCorrectnessProbability)
            .ToList();

        return questions
            .OrderBy(q => orderedKnowledgeSummaryDetails.IndexOf(o => q.Id == o.QuestionId))
            .ToList();
    }
}
