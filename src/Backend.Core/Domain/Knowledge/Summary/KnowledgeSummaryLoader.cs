public class KnowledgeSummaryLoader(KnowledgeSummaryUpdateService knowledgeSummaryUpdateService, KnowledgeSummaryUpdate knowledgeSummaryUpdate) : IRegisterAsInstancePerLifetime
{
    public KnowledgeSummary RunFromCache(int pageId, int userId, int maxCacheAgeInMinutes = 10)
    {
        var knowledgeEvaluationCacheItem = SlidingCache.GetExtendedUserById(userId).GetKnowledgeSummary(pageId);

        if (knowledgeEvaluationCacheItem != null && maxCacheAgeInMinutes > 0)
        {
            if (knowledgeEvaluationCacheItem.LastUpdatedAt >= DateTime.UtcNow.AddMinutes(-maxCacheAgeInMinutes))
            {
                var cachedKnowledgeSummary = knowledgeEvaluationCacheItem.KnowledgeSummary;

                knowledgeSummaryUpdateService.ScheduleUserAndPageUpdate(userId, pageId);

                return cachedKnowledgeSummary;
            }
        }

        var knowledgeSummary = Run(userId, pageId, onlyValuated: false);
        knowledgeSummaryUpdate.UpdateKnowledgeSummary(pageId, userId, knowledgeSummary);

        return knowledgeSummary;
    }

    public KnowledgeSummary Run(int userId, int pageId, bool onlyValuated = true)
    {
        var page = EntityCache.GetPage(pageId);
        if (page == null)
            return new KnowledgeSummary();

        return Run(userId, page.GetAggregatedQuestions(userId).GetIds(), onlyValuated);
    }

    public KnowledgeSummary Run(
        int userId,
        IList<int>? questionIds = null,
        bool onlyValuated = true)
    {
        if (userId <= 0 && questionIds != null)
            return new KnowledgeSummary(notInWishKnowledge: questionIds.Count);

        var questionValuations = SlidingCache.GetExtendedUserById(userId).GetAllQuestionValuations();

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
            notInWishKnowledge: notInWishknowledge);
    }
}