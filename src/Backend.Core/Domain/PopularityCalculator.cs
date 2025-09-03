public class PopularityCalculator : IRegisterAsInstancePerLifetime
{
    /// <summary>
    /// Calculate popularity for a question based on specific point formula:
    /// Views: 1pt, Answers: 2pts each, TotalRelevancePersonalEntries: 5pts each
    /// </summary>
    public int CalculateQuestionPopularity(QuestionCacheItem question)
    {
        if (question == null)
            return 0;

        // Formula: views(1pt) + answers(2pts each) + totalRelevancePersonalEntries(5pts each)
        var popularity = question.TotalViews * 1 +
                         question.TotalAnswers() * 2 +
                         question.TotalRelevancePersonalEntries * 5;

        if (question.Visibility == QuestionVisibility.Public)
            popularity *= 3;

        return Math.Max(0, popularity);
    }

    /// <summary>
    /// Calculate popularity for a page based on page views and normalized question popularity
    /// </summary>
    public int CalculatePagePopularity(PageCacheItem page)
    {
        var directQuestions = page.GetDirectQuestions(onlyVisible: false);
        var allAggregatedQuestions = page.GetAllAggregatedQuestions();

        var normalizedDirectQuestionPopularity = CalculateDirectQuestionPopularityForPage(directQuestions);
        var normalizedIndirectQuestionPopularity =
            CalculateIndirectQuestionPopularityForPage(directQuestions, allAggregatedQuestions);

        var subPageViewCount = GraphService
            .Ascendants(page)
            .Sum(p => p.TotalViews);

        var directViewCount = page.TotalViews - subPageViewCount;

        var childPageViewPopularity = CalculateChildPageViewPopularity(subPageViewCount);

        var totalPopularity = directViewCount +
                              normalizedDirectQuestionPopularity +
                              normalizedIndirectQuestionPopularity +
                              childPageViewPopularity;

        return Math.Max(0, totalPopularity);
    }

    /// <summary>
    /// Calculate normalized popularity from direct questions (higher weight)
    /// </summary>
    private int CalculateDirectQuestionPopularityForPage(IList<QuestionCacheItem> directQuestions)
    {
        var directQuestionPopularity = 0;

        foreach (var question in directQuestions)
        {
            directQuestionPopularity += CalculateQuestionPopularity(question);
        }

        return directQuestionPopularity / 100;
    }

    /// <summary>
    /// Calculate normalized popularity from indirect questions (lower weight)
    /// </summary>
    /// <param name="directQuestions">List of direct questions to exclude</param>
    /// <param name="allAggregatedQuestions">List of all aggregated questions</param>
    /// <returns>Normalized indirect question popularity</returns>
    private int CalculateIndirectQuestionPopularityForPage(IList<QuestionCacheItem> directQuestions,
        IList<QuestionCacheItem> allAggregatedQuestions)
    {
        var indirectQuestionPopularity = 0;
        var directQuestionIds = directQuestions.Select(q => q.Id).ToHashSet();

        var indirectQuestions = allAggregatedQuestions.Where(q => !directQuestionIds.Contains(q.Id));
        foreach (var question in indirectQuestions)
        {
            indirectQuestionPopularity += CalculateQuestionPopularity(question);
        }

        return indirectQuestionPopularity / 300;
    }

    /// <summary>
    /// Calculate the popularity contribution from child page views
    /// </summary>
    /// <param name="page">The parent page</param>
    /// <returns>Weighted child page view popularity</returns>
    private int CalculateChildPageViewPopularity(int subPageViews)
    {
        return (int)(subPageViews * 0.2);
    }
}