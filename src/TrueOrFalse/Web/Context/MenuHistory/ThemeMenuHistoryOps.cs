using System.Collections.Generic;
using System.Linq;

public class ThemeMenuHistoryOps
{
    public static List<Category> GetConnectingCategoryPath(Category firstCategory, Category lastCategory)
    {
        //PATHFINDING HAPPENING HERE
        return new List<Category>();
    }
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