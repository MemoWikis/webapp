public class KnowledgeSummaryLoader(
    PageValuationReadingRepository pageValuationReadingRepository,
    ExtendedUserCache _extendedUserCache) : IRegisterAsInstancePerLifetime
{
    public KnowledgeSummary RunFromDbCache(Page page, int userId)
    {
        var pageValuation = pageValuationReadingRepository.GetBy(page.Id, userId);

        if (pageValuation == null)
        {
            return new KnowledgeSummary(notInWishKnowledge: page.CountQuestionsAggregated);
        }

        return new KnowledgeSummary(
            notLearned: pageValuation.CountNotLearned,
            needsLearning: pageValuation.CountNeedsLearning,
            needsConsolidation: pageValuation.CountNeedsConsolidation,
            solid: pageValuation.CountSolid,
            notInWishKnowledge: Math.Max(0,
                page.CountQuestionsAggregated - pageValuation.CountNotLearned -
                pageValuation.CountNeedsLearning - pageValuation.CountNeedsConsolidation -
                pageValuation.CountSolid)
        );
    }

    public KnowledgeSummary RunFromMemoryCache(int pageId, int userId)
    {
        return RunFromMemoryCache(EntityCache.GetPage(pageId), userId);
    }

    public KnowledgeSummary RunFromMemoryCache(PageCacheItem pageCacheItem, int userId)
    {
        var aggregatedQuestions = new List<QuestionCacheItem>();

        var aggregatedPages = pageCacheItem.AggregatedPages(new PermissionCheck(userId), includingSelf: true);

        foreach (var currentPage in aggregatedPages)
        {
            aggregatedQuestions.AddRange(EntityCache.GetQuestionsForPage(currentPage.Key));
        }

        aggregatedQuestions = aggregatedQuestions.Distinct().ToList();
        var userValuations = _extendedUserCache.GetItem(userId)?.QuestionValuations;
        var aggregatedQuestionValuations = new List<QuestionValuationCacheItem>();
        int countNoValuation = 0;

        foreach (var question in aggregatedQuestions)
        {
            if (userValuations != null && userValuations.ContainsKey(question.Id))
            {
                var valuation = userValuations[question.Id];

                if (valuation != null)
                    aggregatedQuestionValuations.Add(valuation);

                else
                    countNoValuation++;
            }
            else
                countNoValuation++;
        }

        var knowledgeSummary = new KnowledgeSummary(
            notInWishKnowledge: countNoValuation,
            notLearned: aggregatedQuestionValuations.Count(v =>
                v.KnowledgeStatus == KnowledgeStatus.NotLearned),
            needsLearning: aggregatedQuestionValuations.Count(v =>
                v.KnowledgeStatus == KnowledgeStatus.NeedsLearning),
            needsConsolidation: aggregatedQuestionValuations.Count(v =>
                v.KnowledgeStatus == KnowledgeStatus.NeedsConsolidation),
            solid: aggregatedQuestionValuations.Count(v =>
                v.KnowledgeStatus == KnowledgeStatus.Solid)
        );

        return knowledgeSummary;
    }

    public KnowledgeSummary Run(int userId, int pageId, bool onlyValuated = true)
        => Run(userId,
            EntityCache.GetPage(pageId).GetAggregatedQuestionsFromMemoryCache(userId)
                .GetIds(),
            onlyValuated);

    public KnowledgeSummary Run(
        int userId,
        IList<int> questionIds = null,
        bool onlyValuated = true,
        string options = "standard")
    {
        if (userId <= 0 && questionIds != null)
            return new KnowledgeSummary(notInWishKnowledge: questionIds.Count);

        var questionValuations =
            new QuestionValuationCache(_extendedUserCache).GetByUserFromCache(userId);
        if (onlyValuated)
            questionValuations = questionValuations.Where(v => v.IsInWishKnowledge).ToList();
        if (questionIds != null)
            questionValuations = questionValuations.Where(v => questionIds.Contains(v.Question.Id))
                .ToList();

        var notLearned =
            questionValuations.Count(v => v.KnowledgeStatus == KnowledgeStatus.NotLearned);
        var needsLearning =
            questionValuations.Count(v => v.KnowledgeStatus == KnowledgeStatus.NeedsLearning);
        var needsConsolidation =
            questionValuations.Count(v => v.KnowledgeStatus == KnowledgeStatus.NeedsConsolidation);
        var solid = questionValuations.Count(v => v.KnowledgeStatus == KnowledgeStatus.Solid);
        var notInWishknowledge = 0;

        if (questionIds != null)
            notInWishknowledge =
                questionIds.Count - (notLearned + needsLearning + needsConsolidation + solid);

        return new KnowledgeSummary(
            notLearned: notLearned,
            needsLearning: needsLearning,
            needsConsolidation: needsConsolidation,
            solid: solid,
            notInWishKnowledge: notInWishknowledge,
            options: options);
    }
}