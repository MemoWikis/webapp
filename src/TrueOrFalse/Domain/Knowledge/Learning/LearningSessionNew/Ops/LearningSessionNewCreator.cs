using System.Collections.Generic;
using System.Linq;


public class LearningSessionNewCreator
{
    public static LearningSessionNew ForAnonymous(LearningSessionConfig config)
    {
        var questions = GetCategoryQuestionsFromEntityCache(config.CategoryId);
        questions = RandomLimited(questions, config);

        return new LearningSessionNew(questions.Select(q => new LearningSessionStepNew(q)).ToList(), config);
    }

    public static LearningSessionNew ForLoggedInUser(LearningSessionConfig config)
    {  
        List<Question> questions;

        if (config.IsNotQuestionInWishKnowledge && config.CreatedByCurrentUser)
                questions = OrderByProbability(
                        RandomLimited(NotWuwiFromCategoryAndIsAuthor(config.CurrentUserId, config.CategoryId), config))
                    .ToList();
            else if (config.IsNotQuestionInWishKnowledge)
                questions = OrderByProbability(
                    RandomLimited(NotWuwiFromCategory(config.CurrentUserId, config.CategoryId), config)).ToList();
            else if (config.InWishknowledge && config.CreatedByCurrentUser)
                questions = OrderByProbability(RandomLimited(
                    WuwiQuestionsFromCategoryAndUserIsAuthor(config.CurrentUserId, config.CategoryId), config)).ToList();
            else if (config.InWishknowledge)
                questions = OrderByProbability(
                    RandomLimited(WuwiQuestionsFromCategory(config.CurrentUserId, config.CategoryId), config)).ToList();
            else if (config.CreatedByCurrentUser)
                questions = OrderByProbability(
                    RandomLimited(UserIsQuestionAuthor(config.CurrentUserId, config.CategoryId), config)).ToList();
            else
                questions = OrderByProbability(RandomLimited(GetCategoryQuestionsFromEntityCache(config.CategoryId),
                    config)).ToList();

            return new LearningSessionNew(questions.Select(q => new LearningSessionStepNew(q)).ToList(), config);
    }

    public static int GetQuestionCount(LearningSessionConfig config)
    {
        if (config.IsNotQuestionInWishKnowledge && config.CreatedByCurrentUser)
            return NotWuwiFromCategoryAndIsAuthor(config.CurrentUserId, config.CategoryId).Count;
        if (config.IsNotQuestionInWishKnowledge)
            return NotWuwiFromCategory(config.CurrentUserId, config.CategoryId).Count;
        if (config.InWishknowledge && config.CreatedByCurrentUser)
            return WuwiQuestionsFromCategoryAndUserIsAuthor(config.CurrentUserId, config.CategoryId).Count;
        if (config.InWishknowledge)
            return WuwiQuestionsFromCategory(config.CurrentUserId, config.CategoryId).Count;
        if (config.CreatedByCurrentUser)
            return UserIsQuestionAuthor(config.CurrentUserId, config.CategoryId).Count;

        return GetCategoryQuestionsFromEntityCache(config.CategoryId).Count;
    }

    private static List<Question> RandomLimited(List<Question> questions, LearningSessionConfig config)
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

    private static List<Question> WuwiQuestionsFromCategory(int userId, int categoryId)
    {
        return UserCache
            .GetQuestionValuations(userId)
            .Where(qv =>  qv.RelevancePersonal > -1 && qv.Question.Categories.Any(c => c.Id == categoryId))
            .Select(qv => qv.Question)
            .ToList();
    }
    private static List<Question> WuwiQuestionsFromCategoryAndUserIsAuthor(int userId, int categoryId)
    {
        return UserCache
            .GetQuestionValuations(userId)
            .Where(qv => qv.RelevancePersonal > 1 && qv.Question.Categories.Any(c => c.Id == categoryId) && qv.Question.Creator.Id == userId)
            .Select(qv => qv.Question)
            .ToList();
    }


    private static List<Question> NotWuwiFromCategoryAndIsAuthor(int userId, int categoryId)
    {
        return  UserCache
            .GetQuestionValuations(userId)
            .Where(qv => qv.RelevancePersonal == -1 && qv.Question.Categories.Any(c => c.Id == categoryId) && qv.Question.Creator.Id == userId)
            .Select(qv => qv.Question)
            .ToList();
    }

    private static List<Question> NotWuwiFromCategory(int categoryId, int userId)
    {
        return UserCache
            .GetQuestionValuations(userId)
            .Where(qv => qv.RelevancePersonal > 1 && qv.Question.Categories.Any(c => c.Id == categoryId))
            .Select(qv => qv.Question)
            .ToList();
    }

    private static List<Question> UserIsQuestionAuthor(int userId, int categoryId)
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
