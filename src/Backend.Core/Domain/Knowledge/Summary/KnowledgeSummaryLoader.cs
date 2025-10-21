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
            return new KnowledgeSummary(notLearnedInWishKnowledge: questionIds.Count);

        EnsureExtendedUserExists(userId);
        var allQuestionValuations = GetFilteredQuestionValuations(userId, questionIds);
        var totalCounts = CalculateTotalKnowledgeStatusCounts(allQuestionValuations);
        var wishKnowledgeCounts = CalculateWishKnowledgeStatusCounts(allQuestionValuations);
        var adjustedTotalCounts = AdjustForQuestionsWithoutKnowledgeStatus(totalCounts, questionIds, onlyInWishKnowledge);

        // Calculate NotInWishKnowledge counts by subtracting wishKnowledge from total
        var notInWishKnowledgeCounts = new KnowledgeStatusCounts
        {
            NotLearned = adjustedTotalCounts.NotLearned - wishKnowledgeCounts.NotLearned,
            NeedsLearning = adjustedTotalCounts.NeedsLearning - wishKnowledgeCounts.NeedsLearning,
            NeedsConsolidation = adjustedTotalCounts.NeedsConsolidation - wishKnowledgeCounts.NeedsConsolidation,
            Solid = adjustedTotalCounts.Solid - wishKnowledgeCounts.Solid
        };

        return new KnowledgeSummary(
            notLearnedInWishKnowledge: wishKnowledgeCounts.NotLearned,
            needsLearningInWishKnowledge: wishKnowledgeCounts.NeedsLearning,
            needsConsolidationInWishKnowledge: wishKnowledgeCounts.NeedsConsolidation,
            solidInWishKnowledge: wishKnowledgeCounts.Solid,
            notLearnedNotInWishKnowledge: notInWishKnowledgeCounts.NotLearned,
            needsLearningNotInWishKnowledge: notInWishKnowledgeCounts.NeedsLearning,
            needsConsolidationNotInWishKnowledge: notInWishKnowledgeCounts.NeedsConsolidation,
            solidNotInWishKnowledge: notInWishKnowledgeCounts.Solid);
    }

    private void EnsureExtendedUserExists(int userId)
    {
        var extendedUser = SlidingCache.GetExtendedUserByIdNullable(userId);
        if (extendedUser == null)
        {
            extendedUser = _extendedUserCache.CreateExtendedUserCacheItem(userId);
            SlidingCache.AddOrUpdate(extendedUser);
        }
    }

    private List<QuestionValuationCacheItem> GetFilteredQuestionValuations(int userId, IList<int>? questionIds)
    {
        var allQuestionValuations = SlidingCache.GetExtendedUserById(userId).GetAllQuestionValuations();
        
        if (questionIds != null)
            allQuestionValuations = allQuestionValuations.Where(valuation => questionIds.Contains(valuation.Question.Id)).ToList();

        return allQuestionValuations;
    }

    private KnowledgeStatusCounts CalculateTotalKnowledgeStatusCounts(List<QuestionValuationCacheItem> questionValuations)
    {
        return new KnowledgeStatusCounts
        {
            NotLearned = questionValuations.Count(valuation => valuation.KnowledgeStatus == KnowledgeStatus.NotLearned),
            NeedsLearning = questionValuations.Count(valuation => valuation.KnowledgeStatus == KnowledgeStatus.NeedsLearning),
            NeedsConsolidation = questionValuations.Count(valuation => valuation.KnowledgeStatus == KnowledgeStatus.NeedsConsolidation),
            Solid = questionValuations.Count(valuation => valuation.KnowledgeStatus == KnowledgeStatus.Solid)
        };
    }

    private KnowledgeStatusCounts CalculateWishKnowledgeStatusCounts(List<QuestionValuationCacheItem> questionValuations)
    {
        var wishKnowledgeQuestionValuations = questionValuations.Where(valuation => valuation.IsInWishKnowledge).ToList();
        
        return new KnowledgeStatusCounts
        {
            NotLearned = wishKnowledgeQuestionValuations.Count(valuation => valuation.KnowledgeStatus == KnowledgeStatus.NotLearned),
            NeedsLearning = wishKnowledgeQuestionValuations.Count(valuation => valuation.KnowledgeStatus == KnowledgeStatus.NeedsLearning),
            NeedsConsolidation = wishKnowledgeQuestionValuations.Count(valuation => valuation.KnowledgeStatus == KnowledgeStatus.NeedsConsolidation),
            Solid = wishKnowledgeQuestionValuations.Count(valuation => valuation.KnowledgeStatus == KnowledgeStatus.Solid)
        };
    }

    private KnowledgeStatusCounts AdjustForQuestionsWithoutKnowledgeStatus(
        KnowledgeStatusCounts totalCounts, 
        IList<int>? questionIds, 
        bool onlyInWishKnowledge)
    {
        {
            var questionsWithKnowledgeStatus = totalCounts.NotLearned + totalCounts.NeedsLearning + totalCounts.NeedsConsolidation + totalCounts.Solid;
            var questionsWithoutKnowledgeStatus = questionIds.Count - questionsWithKnowledgeStatus;
            
            if (questionsWithoutKnowledgeStatus > 0)
            {
                totalCounts.NotLearned += questionsWithoutKnowledgeStatus;
            }
        }

        return totalCounts;
    }

    private class KnowledgeStatusCounts
    {
        public int NotLearned { get; set; }
        public int NeedsLearning { get; set; }
        public int NeedsConsolidation { get; set; }
        public int Solid { get; set; }
    }
}