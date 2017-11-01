using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;

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
        return RunFromMemoryCache(EntityCache.GetCategory(categoryId), userId);
    }

    public static KnowledgeSummary RunFromMemoryCache(Category category, int userId)
    {
        var stopWatch = Stopwatch.StartNew();

        var aggregatedQuestions = new List<Question>();

        var aggregatedCategories = category.AggregatedCategories(includingSelf: true);

        foreach (var currentCategory in aggregatedCategories)
        {
            aggregatedQuestions.AddRange(EntityCache.GetQuestionsForCategory(currentCategory.Id));
        }

        var aggregatedSets = EntityCache.GetSetsForCategories(aggregatedCategories);

        foreach (var set in aggregatedSets)
        {
            aggregatedQuestions.AddRange(set.Questions());
        }

        aggregatedQuestions = aggregatedQuestions.Distinct().ToList();

        var userValuations = UserValuationCache.GetItem(userId).QuestionValuations;

        var aggregatedQuestionValuations = new List<QuestionValuation>();

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
            aggregatedQuestionValuations.Where(v => v.IsInWishKnowledge()).ToList();

        var knowledgeSummary = new KnowledgeSummary(
            notInWishKnowledge: countNoValuation + aggregatedQuestionValuations.Count(v => !v.IsInWishKnowledge()),
            notLearned: aggregatedQuestionValuationsInWishKnowledge.Count(v => v.KnowledgeStatus == KnowledgeStatus.NotLearned),
            needsLearning: aggregatedQuestionValuationsInWishKnowledge.Count(v => v.KnowledgeStatus == KnowledgeStatus.NeedsLearning),
            needsConsolidation: aggregatedQuestionValuationsInWishKnowledge.Count(v => v.KnowledgeStatus == KnowledgeStatus.NeedsConsolidation),
            solid: aggregatedQuestionValuationsInWishKnowledge.Count(v => v.KnowledgeStatus == KnowledgeStatus.Solid)
            );
        

        Logg.r().Information("Loaded KnowledgeSummary in {Elapsed}", stopWatch.Elapsed);

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
        bool onlyValuated = true)
    {
        if (userId == -1 && questionIds != null)
            return new KnowledgeSummary(notInWishKnowledge: questionIds.Count);

        var queryOver =
            Sl.R<ISession>() 
                .QueryOver<QuestionValuation>()
                .Select(
                    Projections.Group<QuestionValuation>(x => x.KnowledgeStatus),
                    Projections.Count<QuestionValuation>(x => x.KnowledgeStatus)
                )
                .Where(x => x.User.Id == userId);

        if (onlyValuated)
            queryOver.And(x => x.RelevancePersonal != -1);

        if (questionIds != null)
            queryOver.AndRestrictionOn(x => x.Question.Id)
                     .IsIn(questionIds.ToArray());

        var queryResult = queryOver.List<object[]>();

        int notLearned = 0;
        int needsLearning = 0;
        int needsConsolidation = 0;
        int solid = 0;
        int notInWishknowledge = 0;
        foreach (var line in queryResult)
        {
            if ((int) line[0] == (int)KnowledgeStatus.NotLearned)
                notLearned = (int)line[1];

            else if ((int) line[0] == (int)KnowledgeStatus.NeedsLearning)
                needsLearning = (int)line[1];

            else if ((int)line[0] == (int)KnowledgeStatus.NeedsConsolidation)
                needsConsolidation = (int)line[1];

            else if ((int)line[0] == (int)KnowledgeStatus.Solid)
                solid = (int)line[1];
        }

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

    public static KnowledgeSummary Run(
        IList<TrainingQuestion> trainingQuestions, 
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
        foreach (var question in trainingQuestions)
        {
            var value = beforeTraining 
                ? question.ProbBefore 
                : question.ProbAfter;

            if (value < 70)
                needsLearning += 1;
            else if (value < 90)
                needsConsolidation += 1;
            else
                solid += 1;
        }

        return new KnowledgeSummary(needsLearning: needsLearning, needsConsolidation: needsConsolidation, solid: solid);
    }
}

