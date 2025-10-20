public class KnowledgeSummaryLoader(KnowledgeSummaryUpdateDispatcher _knowledgeSummaryUpdateDispatcher, ExtendedUserCache _extendedUserCache) : IRegisterAsInstancePerLifetime
{
    public KnowledgeSummary RunFromCache(int pageId, int userId, int maxCacheAgeInMinutes = 10)
    {
        var knowledgeEvaluationCacheItem = SlidingCache.GetExtendedUserById(userId).GetKnowledgeSummary(pageId);

        if (knowledgeEvaluationCacheItem != null && maxCacheAgeInMinutes > 0)
        {
            if (knowledgeEvaluationCacheItem.DateModified >= DateTime.UtcNow.AddMinutes(-maxCacheAgeInMinutes))
            {
                var cachedKnowledgeSummary = knowledgeEvaluationCacheItem.KnowledgeSummary;

                _knowledgeSummaryUpdateDispatcher.ScheduleUserAndPageUpdateAsync(userId, pageId);

                return cachedKnowledgeSummary;
            }
        }

        var knowledgeSummary = Run(userId, pageId, onlyInWishKnowledge: false);

        SlidingCache.UpdateActiveKnowledgeSummary(userId, pageId, knowledgeSummary);

        return knowledgeSummary;
    }

    public KnowledgeSummary Run(int userId, int pageId, bool onlyInWishKnowledge = true)
    {
        var page = EntityCache.GetPage(pageId);
        if (page == null)
            return new KnowledgeSummary();

        return Run(userId, page.GetAggregatedQuestions(userId).GetIds(), onlyInWishKnowledge);
    }

    public KnowledgeSummary Run(
        int userId,
        IList<int>? questionIds = null,
        bool onlyInWishKnowledge = true)
    {
        if (userId <= 0 && questionIds != null)
            return new KnowledgeSummary(notInWishKnowledge: questionIds.Count);

        var extendedUser = SlidingCache.GetExtendedUserByIdNullable(userId);
        if (extendedUser == null)
        {
            extendedUser = _extendedUserCache.CreateExtendedUserCacheItem(userId);
            SlidingCache.AddOrUpdate(extendedUser);
        }

        var questionValuations = SlidingCache.GetExtendedUserById(userId).GetAllQuestionValuations();

        if (onlyInWishKnowledge)
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
        var notInWishKnowledge = 0;

        if (questionIds != null && !onlyInWishKnowledge)
            notInWishKnowledge = questionIds.Count - (notLearned + needsLearning + needsConsolidation + solid);

        return new KnowledgeSummary(
            notLearned: notLearned,
            needsLearning: needsLearning,
            needsConsolidation: needsConsolidation,
            solid: solid,
            notInWishKnowledge: notInWishKnowledge);
    }
}