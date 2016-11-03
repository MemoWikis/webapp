using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Testing.Values;
using TrueOrFalse.Infrastructure;

public class ContentRecommendation
{
    public static ContentRecommendationResult GetForSet(Set set, int amount = 6, IList<Set> excludeSets = null,
        IList<Category> excludeCategories = null)
    {
        return new ContentRecommendationResult();
    }

    public static ContentRecommendationResult GetForQuestion(Question question, int amount = 6)
    {
        //gets recommended content for a single question. On third of "amount" is each filled with matching Sets (that the question is part of),
        //with matching Categories (with at least 10 questions), and with generally popular Sets
        var result = new ContentRecommendationResult();
        var amountSets = amount <= 3 ? 1 : (int) Math.Ceiling((double)amount/3); //get relevant Sets for the question, at most 1/3 of amount (rounded up)
        var sets = question.SetsTop5;
        sets.Shuffle();
        ((List<Set>)result.Sets).AddRange(sets.Take(amountSets));

        var amountCategories = amount <= 2 ? 0 : (int) Math.Floor((double) amount/3);
        var categories = question.Categories.Where(c => c.CountQuestions > 5).ToList(); //only consider categories with at least 5 questions
        categories.Shuffle();
        ((List<Category>)result.Categories).AddRange(categories.Take(amountCategories));

        var fillUpAmount = amount - result.Sets.Count - result.Categories.Count;
        ((List<Set>)result.PopularSets).AddRange(GetPopularSets(fillUpAmount, excludeSetIds: result.Sets.GetIds()));

        return result;
    }

    public static List<Set> GetPopularSets(int amount, IList<int> excludeSetIds = null, IList<int> avoidSetIds = null)
    {
        //popular sets are by CMS
        if (amount == 0)
            return new List<Set>();

        var suggestedSets = Sl.R<DbSettingsRepo>().Get().SuggestedSets();

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
