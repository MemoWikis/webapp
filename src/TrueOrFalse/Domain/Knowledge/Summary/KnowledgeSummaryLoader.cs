using TrueOrFalse.Domain.Question.QuestionValuation;

public class KnowledgeSummaryLoader : IRegisterAsInstancePerLifetime
{
    private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
    private readonly QuestionValuationReadingRepo _questionValuationReadingRepo;
    private readonly CategoryRepository _categoryRepository;
    private readonly ExtendedUserCache _extendedUserCache;

    public KnowledgeSummaryLoader(
        CategoryValuationReadingRepo categoryValuationReadingRepo,
        QuestionValuationReadingRepo questionValuationReadingRepo,
        CategoryRepository categoryRepository,
        ExtendedUserCache extendedUserCache)
    {
        _categoryValuationReadingRepo = categoryValuationReadingRepo;
        _questionValuationReadingRepo = questionValuationReadingRepo;
        _categoryRepository = categoryRepository;
        _extendedUserCache = extendedUserCache;
    }

    public KnowledgeSummary RunFromDbCache(Category category, int userId)
    {
        var categoryValuation = _categoryValuationReadingRepo.GetBy(category.Id, userId);

        if (categoryValuation == null)
        {
            return new KnowledgeSummary(notInWishKnowledge: category.CountQuestionsAggregated);
        }

        return new KnowledgeSummary(
            notLearned: categoryValuation.CountNotLearned,
            needsLearning: categoryValuation.CountNeedsLearning,
            needsConsolidation: categoryValuation.CountNeedsConsolidation,
            solid: categoryValuation.CountSolid,
            notInWishKnowledge: Math.Max(0,
                category.CountQuestionsAggregated - categoryValuation.CountNotLearned -
                categoryValuation.CountNeedsLearning - categoryValuation.CountNeedsConsolidation -
                categoryValuation.CountSolid)
        );
    }

    public KnowledgeSummary RunFromDbCache(int categoryId, int userId)
    {
        return RunFromDbCache(_categoryRepository.GetById(categoryId), userId);
    }

    public KnowledgeSummary RunFromMemoryCache(int categoryId, int userId)
    {
        return RunFromMemoryCache(EntityCache.GetCategory(categoryId), userId);
    }

    public KnowledgeSummary RunFromMemoryCache(CategoryCacheItem categoryCacheItem, int userId)
    {
        var aggregatedQuestions = new List<QuestionCacheItem>();

        var aggregatedCategories =
            categoryCacheItem
                .AggregatedCategories(new PermissionCheck(userId), includingSelf: true);

        foreach (var currentCategory in aggregatedCategories)
        {
            aggregatedQuestions.AddRange(EntityCache.GetQuestionsForCategory(currentCategory.Key));
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

    public KnowledgeSummary Run(int userId, int categoryId, bool onlyValuated = true)
        => Run(userId,
            EntityCache.GetCategory(categoryId).GetAggregatedQuestionsFromMemoryCache(userId)
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