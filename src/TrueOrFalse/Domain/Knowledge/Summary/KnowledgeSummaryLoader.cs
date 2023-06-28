﻿using System.Collections.Generic;
using System.Linq;

public class KnowledgeSummaryLoader :IRegisterAsInstancePerLifetime
{
    private readonly CategoryValuationRepo _categoryValuationRepo;

    public KnowledgeSummaryLoader(CategoryValuationRepo categoryValuationRepo)
    {
        _categoryValuationRepo = categoryValuationRepo;
    }

    public KnowledgeSummary RunFromDbCache(Category category, int userId)
    {
        var categoryValuation = _categoryValuationRepo.GetBy(category.Id, userId);

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
        return RunFromDbCache(Sl.CategoryRepo.GetById(categoryId), userId);
    }

    public KnowledgeSummary RunFromMemoryCache(int categoryId, int userId)
    {
        return RunFromMemoryCache(EntityCache.GetCategory(categoryId), userId);
    }

    public KnowledgeSummary RunFromMemoryCache(CategoryCacheItem categoryCacheItem, int userId)
    {
        var aggregatedQuestions = new List<QuestionCacheItem>();
        var aggregatedCategories = categoryCacheItem.AggregatedCategories(PermissionCheck.Instance(userId), includingSelf: true);

        foreach (var currentCategory in aggregatedCategories)
        {
            aggregatedQuestions.AddRange(EntityCache.GetQuestionsForCategory(currentCategory.Key));
        }

        aggregatedQuestions = aggregatedQuestions.Distinct().ToList();
        var userValuations = SessionUserCache.GetItem(userId, _categoryValuationRepo)?.QuestionValuations;
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
            notLearned: aggregatedQuestionValuations.Count(v => v.KnowledgeStatus == KnowledgeStatus.NotLearned),
            needsLearning: aggregatedQuestionValuations.Count(v => v.KnowledgeStatus == KnowledgeStatus.NeedsLearning),
            needsConsolidation: aggregatedQuestionValuations.Count(v => v.KnowledgeStatus == KnowledgeStatus.NeedsConsolidation),
            solid: aggregatedQuestionValuations.Count(v => v.KnowledgeStatus == KnowledgeStatus.Solid)
            );
        
        return knowledgeSummary;
    }

    public static KnowledgeSummary Run(int userId, int categoryId, bool onlyValuated = true) 
        => Run(userId, 
            EntityCache.GetCategory(categoryId).GetAggregatedQuestionsFromMemoryCache(userId).GetIds(),
            onlyValuated);

    public static KnowledgeSummary Run(
        int userId, 
        IList<int> questionIds = null, 
        bool onlyValuated = true,
        string options = "standard")
    {
        if (userId <= 0 && questionIds != null)
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
}

