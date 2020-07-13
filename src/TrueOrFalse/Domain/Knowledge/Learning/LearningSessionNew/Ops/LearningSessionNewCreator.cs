using System.Collections.Generic;
using System.Linq;


public class LearningSessionNewCreator
{
    public static LearningSessionNew ForAnonymous(LearningSessionConfig config)
    {
        var questions = GetCategoryQuestionsFromEntityCache(config.CategoryId);
        questions = GetRandomLimited(questions, config);

        return new LearningSessionNew(questions.Select(q => new LearningSessionStepNew(q)).ToList(), config);
    }

    public static LearningSessionNew ForLoggedInUser(LearningSessionConfig config)
    {  
        List<Question> questions;

        if (config.IsNotInWishKnowledge && config.UserIsAuthor)
            questions = OrderByProbability(GetRandomLimited(GetNotWuwiFromCategoryAndIsAuthor(config.UserId, config.CategoryId), config)).ToList();
        else if(config.IsNotInWishKnowledge && config.UserIsAuthor)
            questions = OrderByProbability(GetRandomLimited(GetNotWuwiFromCategory(config.UserId, config.CategoryId), config)).ToList();
        else if(config.IsWishSession && config.UserIsAuthor)
            questions = OrderByProbability(GetRandomLimited(GetWuwiQuestionsFromCategoryAndUserIsAuthor(config.UserId, config.CategoryId), config)).ToList();
        else if (config.IsWishSession)
            questions = OrderByProbability(GetRandomLimited(GetWuwiQuestionsFromCategory(config.UserId, config.CategoryId), config)).ToList();
        else if(config.UserIsAuthor)
            questions = OrderByProbability(GetRandomLimited(UserIsAuthorFromCategory(config.UserId, config.CategoryId), config)).ToList();
        else
            questions = OrderByProbability(GetRandomLimited(GetCategoryQuestionsFromEntityCache(config.CategoryId), config)).ToList();

        return new LearningSessionNew(questions.Select(q => new LearningSessionStepNew(q)).ToList(), config);
    }

    private static List<Question> GetRandomLimited(List<Question> questions, LearningSessionConfig config)
    { 
        if(config.MinProbability != 0 || config.MaxProbability != 100)
            questions = GetQuestionsFromMinToMaxProbability(config.MinProbability, config.MaxProbability, questions);

        questions.Shuffle();

        if (config.MaxQuestions == 0)
            return questions;

        if (config.MaxQuestions > questions.Count)
            return questions;

        var amountQuestionsToDelete = questions.Count - config.MaxQuestions;
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
    private static List<Question> GetWuwiQuestionsFromCategoryAndUserIsAuthor(int userId, int categoryId)
    {
        var l = UserCache.GetQuestionValuations(userId);
        return UserCache
            .GetQuestionValuations(userId)
            .Where(qv => qv.IsInWishKnowledge() && qv.Question.Categories.Any(c => c.Id == categoryId) && qv.Question.Creator.Id == userId)
            .Select(qv => qv.Question)
            .ToList();
    }


    private static List<Question> GetNotWuwiFromCategoryAndIsAuthor(int userId, int categoryId)
    {
        return  UserCache
            .GetQuestionValuations(userId)
            .Where(qv => !qv.IsInWishKnowledge() && qv.Question.Categories.Any(c => c.Id == categoryId) && qv.Question.Creator.Id == userId)
            .Select(qv => qv.Question)
            .ToList();
    }

    private static List<Question> GetNotWuwiFromCategory(int categoryId, int userId)
    {
        return UserCache
            .GetQuestionValuations(userId)
            .Where(qv => !qv.IsInWishKnowledge() && qv.Question.Categories.Any(c => c.Id == categoryId))
            .Select(qv => qv.Question)
            .ToList();
    }

    private static List<Question> UserIsAuthorFromCategory(int userId, int categoryId)
    {
        return UserCache
            .GetQuestionValuations(userId)
            .Where(qv => qv.Question.Categories.Any(c => c.Id == categoryId) && qv.Question.Creator.Id == userId)
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

    private static List<Question> GetQuestionsFromMinToMaxProbability(int minProbability, int maxProbability, List<Question> questions)
    {
        var result = questions.Where(q =>
            q.CorrectnessProbability >= minProbability && q.CorrectnessProbability <= maxProbability);
        return result.ToList(); 
    }
}
