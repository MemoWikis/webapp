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

        if (config.IsNotQuestionInWishKnowledge && config.UserIsAuthor)
            questions = OrderByProbability(GetRandomLimited(GetNotWuwiFromCategoryAndIsAuthor(config.UserId, config.CategoryId), config)).ToList();
        else if(config.IsNotQuestionInWishKnowledge)
            questions = OrderByProbability(GetRandomLimited(GetNotWuwiFromCategory(config.UserId, config.CategoryId), config)).ToList();
        else if(config.QuestionsInWishknowledge && config.UserIsAuthor)
            questions = OrderByProbability(GetRandomLimited(GetWuwiQuestionsFromCategoryAndUserIsAuthor(config.UserId, config.CategoryId), config)).ToList();
        else if (config.QuestionsInWishknowledge)
            questions = OrderByProbability(GetRandomLimited(GetWuwiQuestionsFromCategory(config.UserId, config.CategoryId), config)).ToList();
        else if(config.UserIsAuthor)
            questions = OrderByProbability(GetRandomLimited(UserIsAuthorFromCategory(config.UserId, config.CategoryId), config)).ToList();
        else
            questions = OrderByProbability(GetRandomLimited(GetCategoryQuestionsFromEntityCache(config.CategoryId), config)).ToList();

        return new LearningSessionNew(questions.Select(q => new LearningSessionStepNew(q)).ToList(), config);
    }

    public static int GetQuestionCount(LearningSessionConfig config)
    {
        if (config.IsNotQuestionInWishKnowledge && config.UserIsAuthor)
            return GetNotWuwiFromCategoryAndIsAuthor(config.UserId, config.CategoryId).Count;
        if (config.IsNotQuestionInWishKnowledge)
            return GetNotWuwiFromCategory(config.UserId, config.CategoryId).Count;
        if (config.QuestionsInWishknowledge && config.UserIsAuthor)
            return GetWuwiQuestionsFromCategoryAndUserIsAuthor(config.UserId, config.CategoryId).Count;
        if (config.QuestionsInWishknowledge)
            return GetWuwiQuestionsFromCategory(config.UserId, config.CategoryId).Count;
        if (config.UserIsAuthor)
            return UserIsAuthorFromCategory(config.UserId, config.CategoryId).Count;

        return GetCategoryQuestionsFromEntityCache(config.CategoryId).Count;
    }

    private static List<Question> GetRandomLimited(List<Question> questions, LearningSessionConfig config)
    { 
        if(config.MinProbability != 0 || config.MaxProbability != 100)
            questions = GetQuestionsFromMinToMaxProbability(config.MinProbability, config.MaxProbability, questions);

        questions.Shuffle();

        if (config.MaxQuestionCount == 0)
            return questions;

        if (config.MaxQuestionCount > questions.Count)
            return questions;

        var amountQuestionsToDelete = questions.Count - config.MaxQuestionCount;
        questions.RemoveRange(0, amountQuestionsToDelete);
        
        return questions;
    }

    private static List<Question> GetWuwiQuestionsFromCategory(int userId, int categoryId)
    {
        var l = UserCache.GetQuestionValuations(userId);
        return UserCache
            .GetQuestionValuations(userId)
            .Where(qv =>  qv.RelevancePersonal > -1 && qv.Question.Categories.Any(c => c.Id == categoryId))
            .Select(qv => qv.Question)
            .ToList();
    }
    private static List<Question> GetWuwiQuestionsFromCategoryAndUserIsAuthor(int userId, int categoryId)
    {
        var l = UserCache.GetQuestionValuations(userId);
        return UserCache
            .GetQuestionValuations(userId)
            .Where(qv => qv.RelevancePersonal > 1 && qv.Question.Categories.Any(c => c.Id == categoryId) && qv.Question.Creator.Id == userId)
            .Select(qv => qv.Question)
            .ToList();
    }


    private static List<Question> GetNotWuwiFromCategoryAndIsAuthor(int userId, int categoryId)
    {
        return  UserCache
            .GetQuestionValuations(userId)
            .Where(qv => qv.RelevancePersonal == -1 && qv.Question.Categories.Any(c => c.Id == categoryId) && qv.Question.Creator.Id == userId)
            .Select(qv => qv.Question)
            .ToList();
    }

    private static List<Question> GetNotWuwiFromCategory(int categoryId, int userId)
    {
        return UserCache
            .GetQuestionValuations(userId)
            .Where(qv => qv.RelevancePersonal > 1 && qv.Question.Categories.Any(c => c.Id == categoryId))
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
