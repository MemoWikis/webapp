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

        var knowledgeSummary = Run(userId, pageId, onlyInWishknowledge: false);

        SlidingCache.UpdateActiveKnowledgeSummary(userId, pageId, knowledgeSummary);

        return knowledgeSummary;
    }

    public KnowledgeSummary Run(int userId, int pageId, bool onlyInWishknowledge = true)
    {
        var page = EntityCache.GetPage(pageId);
        if (page == null)
            return new KnowledgeSummary();

        return Run(userId, page.GetAggregatedQuestions(userId).GetIds(), onlyInWishknowledge);
    }

    public KnowledgeSummary Run(
        int userId,
        IList<int>? questionIds = null,
        bool onlyInWishknowledge = true)
    {
        if (userId <= 0 && questionIds != null)
            return new KnowledgeSummary(notLearnedInWishknowledge: questionIds.Count);

        EnsureExtendedUserExists(userId);
        var allQuestionValuations = GetFilteredQuestionValuations(userId, questionIds);
        var totalCounts = CalculateTotalKnowledgeStatusCounts(allQuestionValuations);
        var wishknowledgeCounts = CalculateWishknowledgeStatusCounts(allQuestionValuations);
        var adjustedTotalCounts = AdjustForQuestionsWithoutKnowledgeStatus(totalCounts, questionIds, onlyInWishknowledge);

        // Calculate NotInWishknowledge counts by subtracting wishknowledge from total
        var notInWishknowledgeCounts = new KnowledgeStatusCounts
        {
            NotLearned = adjustedTotalCounts.NotLearned - wishknowledgeCounts.NotLearned,
            NeedsLearning = adjustedTotalCounts.NeedsLearning - wishknowledgeCounts.NeedsLearning,
            NeedsConsolidation = adjustedTotalCounts.NeedsConsolidation - wishknowledgeCounts.NeedsConsolidation,
            Solid = adjustedTotalCounts.Solid - wishknowledgeCounts.Solid
        };

        return new KnowledgeSummary(
            notLearnedInWishknowledge: wishknowledgeCounts.NotLearned,
            needsLearningInWishknowledge: wishknowledgeCounts.NeedsLearning,
            needsConsolidationInWishknowledge: wishknowledgeCounts.NeedsConsolidation,
            solidInWishknowledge: wishknowledgeCounts.Solid,
            notLearnedNotInWishknowledge: notInWishknowledgeCounts.NotLearned,
            needsLearningNotInWishknowledge: notInWishknowledgeCounts.NeedsLearning,
            needsConsolidationNotInWishknowledge: notInWishknowledgeCounts.NeedsConsolidation,
            solidNotInWishknowledge: notInWishknowledgeCounts.Solid);
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

    private KnowledgeStatusCounts CalculateWishknowledgeStatusCounts(List<QuestionValuationCacheItem> questionValuations)
    {
        var wishknowledgeQuestionValuations = questionValuations.Where(valuation => valuation.IsInWishKnowledge).ToList();
        
        return new KnowledgeStatusCounts
        {
            NotLearned = wishknowledgeQuestionValuations.Count(valuation => valuation.KnowledgeStatus == KnowledgeStatus.NotLearned),
            NeedsLearning = wishknowledgeQuestionValuations.Count(valuation => valuation.KnowledgeStatus == KnowledgeStatus.NeedsLearning),
            NeedsConsolidation = wishknowledgeQuestionValuations.Count(valuation => valuation.KnowledgeStatus == KnowledgeStatus.NeedsConsolidation),
            Solid = wishknowledgeQuestionValuations.Count(valuation => valuation.KnowledgeStatus == KnowledgeStatus.Solid)
        };
    }

    private KnowledgeStatusCounts AdjustForQuestionsWithoutKnowledgeStatus(
        KnowledgeStatusCounts totalCounts, 
        IList<int>? questionIds, 
        bool onlyInWishknowledge)
    {
        if (questionIds != null && !onlyInWishknowledge)
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