using System.Linq;

public class ThemeMenuHistoryOps
{
    public static Category GetQuestionSetCategory(int setId)
    {
        var currentSet = Sl.SetRepo.GetById(setId);
        var currentSetCategories = currentSet.Categories;

        var visitedCategories = Sl.SessionUiData.VisitedCategories;

        Category currentCategory;
        if (visitedCategories.Any())
        {
            currentCategory = currentSetCategories.All(c => c.Id == visitedCategories.First().Id)
                ? Sl.CategoryRepo.GetById(visitedCategories.First().Id)
                : currentSetCategories.First();
        }
        else
        {
            currentCategory = currentSetCategories.First();
        }

        return currentCategory;
    }

    public static Category GetQuestionCategory(int questionId)
    {
        var currentQuestion = Sl.QuestionRepo.GetById(questionId);
        var currentQuestionCategories = currentQuestion.Categories;

        var visitedCategories = Sl.SessionUiData.VisitedCategories;

        Category currentCategory;
        if (visitedCategories.Any())
        {
            currentCategory = currentQuestionCategories.All(c => c.Id == visitedCategories.First().Id)
                ? Sl.CategoryRepo.GetById(visitedCategories.First().Id)
                : currentQuestionCategories.First();
        }
        else
        {
            currentCategory = currentQuestionCategories.First();
        }

        return currentCategory;
    }

    public static Category GetTestSessionCategory(int testSessionId)
    {
        var testSession = GetTestSession.Get(testSessionId);
        return testSession.CategoryToTest != null
            ? testSession.CategoryToTest
            : GetQuestionSetCategory(testSession.SetToTest.Id);
    }

    public static Category GetLearningSessionCategory(int learningSessionId)
    {
        var learningSession = Sl.LearningSessionRepo.GetById(learningSessionId);
        return learningSession.CategoryToLearn != null
            ? learningSession.CategoryToLearn
            : GetQuestionSetCategory(learningSession.SetToLearn.Id);
    }

}