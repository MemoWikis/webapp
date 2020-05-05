using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Seedworks.Web.State;

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
        List<Question> questions;
        if (config.OnlyWuwi)
            questions = OrderByProbability(GetWuwiQuestionsFromCategory(config.UserId, config.CategoryId)).ToList();
        else
            questions = OrderByProbability(GetCategoryQuestionsFromEntityCache(config.CategoryId)).ToList();

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

    private static List<Question> GetWuwiQuestionsFromCategory(int userId, int categoryId)
    {
        var l = UserCache.GetQuestionValuations(userId);
        return UserCache
            .GetQuestionValuations(userId)
            .Where(qv =>  qv.IsInWishKnowledge() && qv.Question.Categories.Any(c => c.Id == categoryId))
            .Select(qv => qv.Question)
            .ToList();
    }

    private static List<Question> GetCategoryQuestionsFromEntityCache(int categoryId)
    {
        return EntityCache.GetQuestionsForCategory(categoryId).ToList();
    }

    private static IList<Question> OrderByProbability( List<Question> questions)
    {
        return questions.OrderByDescending(q => q.CorrectnessProbability).ToList();
    }
}
