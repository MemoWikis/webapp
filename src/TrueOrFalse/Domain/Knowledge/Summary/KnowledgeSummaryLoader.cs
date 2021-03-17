using System;
using System.Collections.Generic;
using System.Linq;

public class KnowledgeSummaryLoader
{
    public static KnowledgeSummary RunFromDbCache(Category category, int userId)
    {
        var categoryValuation = Sl.CategoryValuationRepo.GetBy(category.Id, userId);

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

    public static KnowledgeSummary RunFromDbCache(int categoryId, int userId)
    {
        return RunFromDbCache(Sl.CategoryRepo.GetById(categoryId), userId);
    }

    public static KnowledgeSummary RunFromMemoryCache(int categoryId, int userId)
    {
        return RunFromMemoryCache(EntityCache.GetCategoryCacheItem(categoryId), userId);
    }

    public static KnowledgeSummary RunFromMemoryCache(Category category, int userId)
    {
        var aggregatedQuestions = new List<Question>();

        var aggregatedCategories = category.AggregatedCategories(includingSelf: true);

        foreach (var currentCategory in aggregatedCategories)
        {
            aggregatedQuestions.AddRange(EntityCache.GetQuestionsForCategory(currentCategory.Id));
        }

        aggregatedQuestions = aggregatedQuestions.Distinct().ToList();

        var userValuations = UserCache.GetItem(userId).QuestionValuations;
        var aggregatedQuestionValuations = new List<QuestionValuationCacheItem>();
        int countNoValuation = 0;

        foreach (var question in aggregatedQuestions)
        {
            if (userValuations.ContainsKey(question.Id))
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

        var aggregatedQuestionValuationsInWishKnowledge =
            aggregatedQuestionValuations.Where(v => v.IsInWishKnowledge).ToList();

        var knowledgeSummary = new KnowledgeSummary(
            notInWishKnowledge: countNoValuation + aggregatedQuestionValuations.Count(v => !v.IsInWishKnowledge),
            notLearned: aggregatedQuestionValuationsInWishKnowledge.Count(v => v.KnowledgeStatus == KnowledgeStatus.NotLearned),
            needsLearning: aggregatedQuestionValuationsInWishKnowledge.Count(v => v.KnowledgeStatus == KnowledgeStatus.NeedsLearning),
            needsConsolidation: aggregatedQuestionValuationsInWishKnowledge.Count(v => v.KnowledgeStatus == KnowledgeStatus.NeedsConsolidation),
            solid: aggregatedQuestionValuationsInWishKnowledge.Count(v => v.KnowledgeStatus == KnowledgeStatus.Solid)
            );
        
        return knowledgeSummary;
    }

    public static KnowledgeSummary Run(int userId, int categoryId, bool onlyValuated = true) 
        => Run(userId, 
            Sl.CategoryRepo.GetById(categoryId).GetAggregatedQuestionsFromMemoryCache().GetIds(),
            onlyValuated);

    public static KnowledgeSummary Run(int userId, Set set, bool onlyValuated = true) 
        => Run(userId, set.QuestionIds(), onlyValuated);

    public static KnowledgeSummary Run(
        int userId, 
        IList<int> questionIds = null, 
        bool onlyValuated = true,
        string options = "standard")
    {
        if (userId == -1 && questionIds != null)
            return new KnowledgeSummary(notInWishKnowledge: questionIds.Count);

        var questionValuations = Sl.QuestionValuationRepo.GetByUserFromCache(userId);
        if (onlyValuated)
            questionValuations = questionValuations.Where(v => v.IsInWishKnowledge).ToList();
        if (questionIds != null)
            questionValuations = questionValuations.Where(v => questionIds.Contains(v.Question.Id)).ToList();

        var notLearned = questionValuations.Count(v => v.KnowledgeStatus == KnowledgeStatus.NotLearned);
        var needsLearning = questionValuations.Count(v => v.KnowledgeStatus == KnowledgeStatus.NeedsLearning);
        var needsConsolidation = questionValuations.Count(v => v.KnowledgeStatus == KnowledgeStatus.NeedsConsolidation);
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

    public static KnowledgeSummary Run(
        bool beforeTraining = false, 
        bool afterTraining = false)
    {
        if(beforeTraining && afterTraining)
            throw new Exception();

        if (!beforeTraining && !afterTraining)
            throw new Exception();

        var needsLearning = 0;
        var needsConsolidation = 0;
        var solid = 0;

        return new KnowledgeSummary(needsLearning: needsLearning, needsConsolidation: needsConsolidation, solid: solid);
    }
}

