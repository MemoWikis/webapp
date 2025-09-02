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

        var normalizedDirectQuestionPopularity = CalculateDirectQuestionPopularity(directQuestions);
        var normalizedIndirectQuestionPopularity = CalculateIndirectQuestionPopularity(directQuestions, allAggregatedQuestions);
        var childPageViewPopularity = CalculateChildPageViewPopularity(page);

        var totalPopularity = page.TotalViews + 
                             normalizedDirectQuestionPopularity + 
                             normalizedIndirectQuestionPopularity + 
                             childPageViewPopularity;

        return Math.Max(0, totalPopularity);
    }

    /// <summary>
    /// Calculate normalized popularity from direct questions (higher weight)
    /// </summary>
    private int CalculateDirectQuestionPopularity(IList<QuestionCacheItem> directQuestions)
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
    private int CalculateIndirectQuestionPopularity(IList<QuestionCacheItem> directQuestions, IList<QuestionCacheItem> allAggregatedQuestions)
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
    private int CalculateChildPageViewPopularity(PageCacheItem page)
    {
        var childPages = page.GetAllAggregatedPages(includingSelf: false);
        var totalChildPopularity = 0;

        foreach (var childPage in childPages.Values)
        {
            // Only count direct page views from children, not their questions
            // to avoid exponential complexity
            totalChildPopularity += childPage.TotalViews;
        }

        // Apply a factor to reduce child contribution (0.2 = 20% weight)
        // This prevents child pages from overwhelming the parent's own popularity
        return (int)(totalChildPopularity * 0.2);
    }
}