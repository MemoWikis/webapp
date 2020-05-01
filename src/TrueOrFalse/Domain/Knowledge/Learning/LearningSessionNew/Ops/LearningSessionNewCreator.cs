using System.Collections.Generic;
using System.Linq;

public class LearningSessionNewCreator
{
    public static LearningSessionNew ForAnonymous(LearningSessionConfig config)
    {
        var questions = GetCategoryQuestionsFromEntityCache(config.CategoryId);
        questions = GetRandomLimited(questions, config.MaxQuestions);

        return new LearningSessionNew
        {
            Config = config,
            Steps = questions.Select(q => new LearningSessionStepNew(q)).ToList()
        };
    }

    public static LearningSessionNew ForLoggedInUser(LearningSessionConfig config)
    {
        var questions = DiffculitiFirst(GetCategoryQuestionsFromEntityCache(config.CategoryId));

        //Repeat wrong answers
        return new LearningSessionNew
        {
            Config = config,
            Steps = questions.Select(q => new LearningSessionStepNew(q)).ToList() 
        };
    }

    private static List<Question> GetRandomLimited(List<Question> questions, int amountQuestions)
    {
        questions.Shuffle();

        if (amountQuestions == 0)
            return questions;

        if (amountQuestions > questions.Count)
            return questions;

        var amountQuestionsToDelete = questions.Count - amountQuestions;
        questions.RemoveRange(0, amountQuestionsToDelete);
        
        return questions;
    }

    private List<Question> GetWuwiQuestionsFromCategory(int userId, int categoryId)
    {
        return UserCache
            .GetQuestionValuations(userId)
            .Where(qv => qv.Question.Categories.Any(c => c.Id == categoryId))
            .Select(qv => qv.Question)
            .ToList();
    }

    private static List<Question> GetCategoryQuestionsFromEntityCache(int categoryId)
    {
        return EntityCache.GetQuestionsForCategory(categoryId).ToList();
    }

    private static IList<Question> DiffculitiFirst( List<Question> questions)
    {
        return questions.OrderByDescending(q => q.CorrectnessProbability).ToList();

    }
}
