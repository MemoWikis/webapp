public class PopularityCalculator : IRegisterAsInstancePerLifetime
{
    public PopularityCalculator()
    {
    }

    /// <summary>
    /// Calculate popularity for a question based on specific point formula:
    /// Views: 1pt, Answers: 2pts each, TotalRelevancePersonalEntries: 5pts each
    /// </summary>
    /// <param name="question">The question to calculate popularity for</param>
    /// <returns>Popularity score as integer</returns>
    public int CalculateQuestionPopularity(QuestionCacheItem question)
    {
        if (question == null)
            return 0;

        // Formula: views(1pt) + answers(2pts each) + totalRelevancePersonalEntries(5pts each)
        var popularity = question.TotalViews * 1 +                           // Views: 1 point each
                        question.TotalAnswers() * 2 +                        // Answers: 2 points each
                        question.TotalRelevancePersonalEntries * 5;          // TotalRelevancePersonalEntries: 5 points each

        return Math.Max(0, popularity); // Ensure non-negative
    }

    /// <summary>
    /// Calculate popularity for a page based on total view count
    /// </summary>
    /// <param name="page">The page to calculate popularity for</param>
    /// <returns>Popularity score as integer</returns>
    public int CalculatePagePopularity(PageCacheItem page)
    {
        if (page == null)
            return 0;

        // Use total view count as popularity
        return Math.Max(0, page.TotalViews);
    }
}
