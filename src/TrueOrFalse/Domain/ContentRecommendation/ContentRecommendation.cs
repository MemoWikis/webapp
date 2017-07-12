using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Testing.Values;
using TrueOrFalse.Infrastructure;

public class ContentRecommendation
{
    public static ContentRecommendationResult GetForSet(Set set, int amount)
    {
        var result = new ContentRecommendationResult();
        var amountSets = amount <= 3 ? 1 : (int)Math.Ceiling((double)amount / 3); //get at most 1/3 of amount (rounded up) sets
        var sets = set.Questions().SelectMany(q => q.SetsTop5).Distinct().ToList();
        sets.RemoveAll(s => s == set);
        sets.Shuffle();
        ((List<Set>)result.Sets).AddRange(sets.Take(amountSets));

        var amountCategories = amount <= 2 ? 0 : (int)Math.Floor((double)amount / 3);
        var categories = set.Questions().SelectMany(q => q.Categories).Distinct().Where(c => c.CountQuestionsAggregated > 5 || c.CountQuestions > 5).ToList(); //only consider categories with at least 5 questions
        categories.Shuffle();
        ((List<Category>)result.Categories).AddRange(categories.Take(amountCategories));

        var fillUpAmount = amount - result.Sets.Count - result.Categories.Count;
        var excludeSetIds = result.Sets.GetIds();
        excludeSetIds.Add(set.Id);
        ((List<Set>)result.PopularSets).AddRange(GetPopularSets(fillUpAmount, excludeSetIds: excludeSetIds));

        return result;
    }

    public static ContentRecommendationResult GetForCategory(Category category, int amount)
    {
        var result = new ContentRecommendationResult();

        var amountSets = amount <= 3 ? 1 : (int)Math.Ceiling((double)amount / 3); //get at most 1/3 of amount (rounded up) sets
        var questions = Sl.R<QuestionRepo>().GetForCategory(category.Id);

        var sets = questions.SelectMany(q => q.SetsTop5).Distinct().ToList();
        sets.AddRange(Sl.R<SetRepo>().GetForCategory(category.Id));
        sets.Shuffle();
        ((List<Set>)result.Sets).AddRange(sets.Take(amountSets));

        var amountCategories = amount <= 2 ? 0 : (int)Math.Floor((double)amount / 3);
        var categories = Sl.R<CategoryRepository>().GetChildren(category.Id);
        ((List<Category>)categories).AddRange(category.ParentCategories());
        //not yet included: "sibling"-categories (= children of parent categories).
        categories = categories.Distinct().ToList();
        ((List<Category>)categories).RemoveAll(c => Math.Max(c.CountQuestionsAggregated, c.CountQuestions) < 5 || c == category); //only consider categories with at least 5 questions

        categories.Shuffle();
        ((List<Category>)result.Categories).AddRange(categories.Take(amountCategories));

        var fillUpAmount = amount - result.Sets.Count - result.Categories.Count;
        ((List<Set>)result.PopularSets).AddRange(GetPopularSets(fillUpAmount, excludeSetIds: result.Sets.GetIds()));

        return result;
    }

    public static ContentRecommendationResult GetForQuestion(Question question, int amount = 6)
    {
        //gets recommended content for a single question. On third of "amount" is each filled with matching Sets (that the question is part of),
        //with matching Categories (with at least 10 questions), and with generally popular Sets
        var result = new ContentRecommendationResult();
        var amountSets = amount <= 3 ? 1 : (int) Math.Ceiling((double)amount/3); //get at most 1/3 of amount (rounded up) sets
        var sets = question.SetsTop5;
        sets.Shuffle();
        ((List<Set>)result.Sets).AddRange(sets.Take(amountSets));

        var amountCategories = amount <= 2 ? 0 : (int) Math.Floor((double) amount/3);
        var categories = question.Categories.Where(c => c.CountQuestionsAggregated > 5 || c.CountQuestions > 5).ToList(); //only consider categories with at least 5 questions
        categories.Shuffle();
        ((List<Category>)result.Categories).AddRange(categories.Take(amountCategories));

        var fillUpAmount = amount - result.Sets.Count - result.Categories.Count;
        ((List<Set>)result.PopularSets).AddRange(GetPopularSets(fillUpAmount, excludeSetIds: result.Sets.GetIds()));

        return result;
    }

    public static List<Set> GetPopularSets(int amount, IList<int> excludeSetIds = null, IList<int> avoidSetIds = null)
    {
        //popular sets are: all those created by memucho user and those set in CMS (settings table: suggested sets)
        if (amount == 0)
            return new List<Set>();

        var suggestedSets = Sl.R<DbSettingsRepo>().Get().SuggestedSets();
        var setsByMemucho = Sl.R<SetRepo>().GetByCreatorId(Settings.MemuchoUserId);

        ((List<Set>)suggestedSets).AddRange(setsByMemucho);
        suggestedSets = suggestedSets.Distinct().ToList();

        if ((excludeSetIds != null) && excludeSetIds.Any())
            suggestedSets.RemoveAll(s => excludeSetIds.Contains(s.Id));

        if ((avoidSetIds != null) && avoidSetIds.Any() && (suggestedSets.Count > amount))
        {
            avoidSetIds.Shuffle();
            var index = 0;
            while ((suggestedSets.Count > amount) && (index < avoidSetIds.Count))
            {
                suggestedSets.RemoveAll(s => s.Id == avoidSetIds.ElementAt(index));
                index++;
            }
        }
        suggestedSets.Shuffle();
        return suggestedSets.Take(amount).ToList();
    }
}
