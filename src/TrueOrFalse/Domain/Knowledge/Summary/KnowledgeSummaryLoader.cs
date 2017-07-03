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
            return new KnowledgeSummary
            {
                NotInWishknowledge = category.CountQuestionsAggregated
            };
        }

        return new KnowledgeSummary
        {
            NotLearned = categoryValuation.CountNotLearned,
            NeedsLearning = categoryValuation.CountNeedsLearning,
            NeedsConsolidation = categoryValuation.CountNeedsConsolidation,
            Solid = categoryValuation.CountSolid,
            NotInWishknowledge = Math.Max(0,
                category.CountQuestionsAggregated - categoryValuation.CountNotLearned -
                categoryValuation.CountNeedsLearning - categoryValuation.CountNeedsConsolidation -
                categoryValuation.CountSolid)
        };
    }

    public static KnowledgeSummary RunFromDbCache(int categoryId, int userId)
    {
        return RunFromDbCache(Sl.CategoryRepo.GetById(categoryId), userId);
    }

    public static KnowledgeSummary RunFromMemoryCache(int categoryId, int userId)
    {
        return RunFromMemoryCache(EntityCache.Categories[categoryId], userId);
    }

    public static KnowledgeSummary RunFromMemoryCache(Category category, int userId)
    {
        //Other Todos:
        // - update category cache, on category changes (in repo)
        // - update category relations cache, on relation changes (in repo)

        var stopWatch = Stopwatch.StartNew();

        var aggregatedQuestions = new List<Question>();

        var aggregatedCategories = AggregatedCategoryLoader.FromCache(category);

        foreach (var currentCategory in aggregatedCategories)
        {
            if(EntityCache.CategoryQuestionsList.ContainsKey(currentCategory.Id))
                aggregatedQuestions.AddRange(EntityCache.CategoryQuestionsList[currentCategory.Id].Select(c => c.Value));
        }

        var aggregatedSets = GetAllSetsWithAssociatedCategories(aggregatedCategories);

        foreach (var set in aggregatedSets)
        {
            aggregatedQuestions.AddRange(set.Value.Questions());
        }

        aggregatedQuestions = aggregatedQuestions.Distinct().ToList();

        var aggregatedQuestionValuations = UserValuationCache.GetQuestionValuations(userId)
            .Where(v => aggregatedQuestions.Any(q => q == v.Question)).ToList();

        var aggregatedQuestionValuationsInWishKnowledge =
            aggregatedQuestionValuations.Where(v => v.IsInWishKnowledge()).ToList();

        var knowledgeSummary = new KnowledgeSummary
        {
            NotInWishknowledge = aggregatedQuestionValuations.Count(v => !v.IsInWishKnowledge()),
            NotLearned = aggregatedQuestionValuationsInWishKnowledge.Count(v => v.KnowledgeStatus == KnowledgeStatus.NotLearned),
            NeedsLearning = aggregatedQuestionValuationsInWishKnowledge.Count(v => v.KnowledgeStatus == KnowledgeStatus.NeedsLearning),
            NeedsConsolidation = aggregatedQuestionValuationsInWishKnowledge.Count(v => v.KnowledgeStatus == KnowledgeStatus.NeedsConsolidation),
            Solid = aggregatedQuestionValuationsInWishKnowledge.Count(v => v.KnowledgeStatus == KnowledgeStatus.Solid),
        };

        Logg.r().Information("Loaded KnowledgeSummary in {Elapsed}", stopWatch.Elapsed);

        return knowledgeSummary;
    }

    private static IEnumerable<KeyValuePair<int, Set>> GetAllSetsWithAssociatedCategories(IList<Category> aggregatedCategories)
    {
        return EntityCache.Sets.Where(s => s.Value.Categories.Any(c => aggregatedCategories.Any(ac => c == ac)));
    }

    public static KnowledgeSummary Run(int userId, int categoryId, bool onlyValuated = true) 
        => Run(userId, 
            Sl.CategoryRepo.GetById(categoryId).GetAggregatedQuestions().GetIds(),
            onlyValuated);

    public static KnowledgeSummary Run(int userId, Set set, bool onlyValuated = true) 
        => Run(userId, set.QuestionIds(), onlyValuated);

    public static KnowledgeSummary Run(
        int userId, 
        IList<int> questionIds = null, 
        bool onlyValuated = true)
    {
        if (userId == -1 && questionIds != null)
            return new KnowledgeSummary {NotInWishknowledge = questionIds.Count};

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

        var result = new KnowledgeSummary();

        foreach (var line in queryResult)
        {
            if ((int) line[0] == (int)KnowledgeStatus.NotLearned)
                result.NotLearned = (int)line[1];

            else if ((int) line[0] == (int)KnowledgeStatus.NeedsLearning)
                result.NeedsLearning = (int)line[1];

            else if ((int)line[0] == (int)KnowledgeStatus.NeedsConsolidation)
                result.NeedsConsolidation = (int)line[1];

            else if ((int)line[0] == (int)KnowledgeStatus.Solid)
                result.Solid = (int)line[1];
        }

        if (questionIds != null)
            result.NotInWishknowledge = 
                questionIds.Count - (result.NotLearned + result.NeedsLearning + result.NeedsConsolidation + result.Solid);

        return result;
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

        var result = new KnowledgeSummary();

        foreach (var question in trainingQuestions)
        {
            var value = beforeTraining 
                ? question.ProbBefore 
                : question.ProbAfter;

            if (value < 70)
                result.NeedsLearning += 1;
            else if (value < 90)
                result.NeedsConsolidation += 1;
            else 
                result.Solid += 1;
        }

        return result;
    }
}

