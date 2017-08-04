using System.Collections.Generic;
using System.Linq;

public class ThemeMenuHistoryOps
{
    public static List<Category> GetConnectedCategoryPath(List<Category> path, Category upperCategory)
    {
        var pathParents = path.First().ParentCategories();
        foreach (var pathParent in pathParents)
        {
            var newPath = new List<Category>(path);
            newPath.Insert(0, pathParent);
            if (newPath.First() == upperCategory)
                return newPath;

            var finalParentPath = GetConnectedCategoryPath(newPath, upperCategory);
            if (finalParentPath.Count > 0)
                return finalParentPath;
        }
        if (path.Last() == upperCategory)
            return new List<Category> { upperCategory };

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