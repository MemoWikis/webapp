using System;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Infrastructure;

public class ContentRecommendation
{
    public static ContentRecommendationResult GetForSet(Set set, int amount)
    {
        var result = new ContentRecommendationResult();
        var amountCategories = amount <= 2 ? 0 : (int)Math.Floor((double)amount / 3);
        var categories = set.Questions().SelectMany(q => q.CategoriesIds).Distinct().Where(c => c.CountQuestionsAggregated > 5 || c.CountQuestions > 5).ToList(); //only consider categories with at least 5 questions
        categories.Shuffle();
        ((List<Category>)result.Categories).AddRange(categories.Take(amountCategories));
        return result;
    }

    public static ContentRecommendationResult GetForQuestion(Question question, int amount = 6)
    {
        //gets recommended content for a single question. On third of "amount" is each filled with matching Sets (that the question is part of),
        //with matching Categories (with at least 10 questions), and with generally popular Sets
        var result = new ContentRecommendationResult();

        var amountCategories = amount <= 2 ? 0 : (int) Math.Floor((double) amount/3);
        var categories = question.CategoriesIds.Where(c => c.CountQuestionsAggregated > 5 || c.CountQuestions > 5).ToList(); //only consider categories with at least 5 questions
        categories.Shuffle();
        ((List<Category>)result.Categories).AddRange(categories.Take(amountCategories));

        return result;
    }
}
