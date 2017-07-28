using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ThemeMenuHistoryOps
{
    //public static Category GetQuestionSetCategory(int setId)
    //{
    //    var currentSet = Sl.SetRepo.GetById(setId);
    //    var currentSetCategories = currentSet.Categories;

    //    var visitedCategories = Sl.SessionUiData.VisitedCategories;

    //    Category currentCategory;
    //    if (visitedCategories.Any())
    //    {
    //        currentCategory = currentSetCategories.All(c => c.Id == visitedCategories.First().Id)
    //            ? Sl.CategoryRepo.GetById(visitedCategories.First().Id)
    //            : currentSetCategories.First();
    //    }
    //    else
    //    {
    //        currentCategory = currentSetCategories.First();
    //    }

    //    return currentCategory;
    //}

    public static List<Category> GetQuestionCategories(int questionId)
    {
        var question = Sl.QuestionRepo.GetById(questionId);

        if (question.Categories.Count > 0)
        {
            return question.Categories.ToList();
        }

        var questionSetsCategories = new List<Category>();
        questionSetsCategories.AddRange(question.SetsTop5.SelectMany(s => s.Categories));
        return questionSetsCategories;
    }

    //public static Category GetTestSessionCategory(int testSessionId)
    //{
    //}

    //public static Category GetLearningSessionCategory(int learningSessionId)
    //{
    //}

}