using System.Collections.Generic;
using System.Linq;

public class GetAllSetsWithDifferentlyCategorizedQuestions
{
    public static List<Set> Run()
    {
        var result = new List<Set>();
        var sets = Sl.SetRepo.GetAllEager();
        foreach (var set in sets)
        {
            if (SetHasQuestionWithDifferentCategories(set))
                result.Add(set);
        }
        return result;
    }

    private static bool SetHasQuestionWithDifferentCategories(Set set)
    {
        foreach (var question in set.Questions())
        {
            if (HasQuestionDifferentCategoriesThanItsSets(question))
                return true;
        }
        return false;
    }

    private static bool HasQuestionDifferentCategoriesThanItsSets(Question question)
    {
        var questionCategories = question.Categories;
        var sets = new List<Set>();
        if (question.SetsAmount <= 5)
        {
            sets = question.SetsTop5.ToList();
        }
        else
        {
            var questionInSets = Sl.R<QuestionInSetRepo>()
                .Query
                .Where(x => x.Question.Id == question.Id)
                .List();

            sets = questionInSets
                .Where(x => x.Set != null)
                .GroupBy(x => x.Set.Id)
                .Select(x => x.First())
                .Select(x => x.Set)
                .ToList();
        }
        var setCategories = new List<Category>();
        sets.ForEach(s => setCategories.AddRange(s.Categories));
        var questionCategoriesIds = questionCategories.Select(c => c.Id).OrderBy(c => c).ToList();
        var setCategoriesIds = setCategories.Select(c => c.Id).OrderBy(c => c).ToList();
        return !questionCategoriesIds.SequenceEqual(setCategoriesIds);
    }
}
