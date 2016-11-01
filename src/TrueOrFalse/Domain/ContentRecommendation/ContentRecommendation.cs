using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Testing.Values;

class ContentRecommendation
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
        ((List<Set>)result.Sets).AddRange(question.SetsTop5.Take(amountSets));

        var amountCategories = amount <= 2 ? 0 : (int) Math.Floor((double) amount/3);
        //todo: check to display only categories with at least 10 questions
        ((List<Category>)result.Categories).AddRange(question.Categories.Take(amountCategories));
        var fillUpAmount = amount - result.Sets.Count - result.Categories.Count;
        ((List<Set>)result.PopularSets).AddRange(GetPopularSets(fillUpAmount, excludeSets: result.Sets));
        return result;
    }

    public static List<Set> GetPopularSets(int amount, IList<Set> excludeSets = null, IList<Set> avoidSets = null)
    {
        //popular sets are by CMS; get them by shuffling
        //if (amount == 0)
            return new List<Set>();
        

    }
}
