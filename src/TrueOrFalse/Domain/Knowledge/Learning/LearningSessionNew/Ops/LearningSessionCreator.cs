using System.Collections.Generic;
using System.Linq;

public class LearningSessionCreator
{
    public static LearningSession ForAnonymous(LearningSessionConfig config)
    {
        var questions = RandomLimited(GetCategoryQuestionsFromEntityCache(config.CategoryId), config);

        return new LearningSession(questions.Select(q => new LearningSessionStep(q)).ToList(), config);
    }

    public static LearningSession ForLoggedInUser(LearningSessionConfig config)
    {  
        List<Question> questions = new List<Question>();
        if (UserCache.GetItem(config.CurrentUserId).IsFiltered)
        {
            var questionsFromCurrentCategoryAndChildren = GetCategoryQuestionsFromEntityCache(config.CategoryId);  
            questions = questionsFromCurrentCategoryAndChildren.Distinct().ToList(); 
        }
        else if (config.AllQuestions || config.InWishknowledge && config.CreatedByCurrentUser && config.IsNotQuestionInWishKnowledge)
            questions = OrderByProbability(RandomLimited(GetCategoryQuestionsFromEntityCache(config.CategoryId),
                config)).ToList();
        else if (config.IsNotQuestionInWishKnowledge && config.InWishknowledge && !config.CreatedByCurrentUser) 
            questions = RandomLimited(IsInWuWiFromCategoryAndIsNotInWuwi(config.CurrentUserId, config.CategoryId), config);
        else if(config.IsNotQuestionInWishKnowledge && config.CreatedByCurrentUser)
            questions = OrderByProbability(
                    RandomLimited(NotWuwiFromCategoryOrIsAuthor(config.CurrentUserId, config.CategoryId), config))
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

        return new LearningSession(questions.Select(q => new LearningSessionStep(q)).ToList(), config);
    }

    public static int GetQuestionCount(LearningSessionConfig config)
    {
        config.MaxQuestionCount = 0;
        if(config.AllQuestions || config.InWishknowledge && config.CreatedByCurrentUser && config.IsNotQuestionInWishKnowledge)
            return RandomLimited(GetCategoryQuestionsFromEntityCache(config.CategoryId), config).Count;
        if (config.IsNotQuestionInWishKnowledge && config.InWishknowledge && !config.CreatedByCurrentUser)
            return RandomLimited(IsInWuWiFromCategoryAndIsNotInWuwi(config.CurrentUserId, config.CategoryId),config).Count; 
        if (config.IsNotQuestionInWishKnowledge && config.CreatedByCurrentUser)
            return RandomLimited(NotWuwiFromCategoryOrIsAuthor(config.CurrentUserId, config.CategoryId),config).Count;
        if (config.IsNotQuestionInWishKnowledge)
            return RandomLimited(NotWuwiFromCategory(config.CurrentUserId, config.CategoryId),config).Count;
        if (config.InWishknowledge && config.CreatedByCurrentUser)
            return RandomLimited(WuwiQuestionsFromCategoryAndUserIsAuthor(config.CurrentUserId, config.CategoryId),config).Count;
        if (config.InWishknowledge)
            return RandomLimited(WuwiQuestionsFromCategory(config.CurrentUserId, config.CategoryId),config).Count;
        if (config.CreatedByCurrentUser)
            return RandomLimited(UserIsQuestionAuthor(config.CurrentUserId, config.CategoryId),config).Count;

        return 0; 
    }

    private static List<Question> RandomLimited(List<Question> questions, LearningSessionConfig config)
    { 
        if(config.MinProbability != 0 || config.MaxProbability != 100)
            questions = GetQuestionsFromMinToMaxProbability(config.MinProbability, config.MaxProbability, questions);

        questions.Shuffle();

        if (config.MaxQuestionCount == 0)
            return questions;

        if (config.MaxQuestionCount > questions.Count || config.MaxQuestionCount == -1)
            return questions;

        var amountQuestionsToDelete = questions.Count - config.MaxQuestionCount;
        questions.RemoveRange(0, amountQuestionsToDelete);
        
        return questions;
    }

    private static List<Question> WuwiQuestionsFromCategory(int userId, int categoryId)
    {
        return CompareDictionaryWithQuestionsFromMemoryCache(GetIdsFromQuestionValuation(userId), categoryId);
    }

    private static List<Question> WuwiQuestionsFromCategoryAndUserIsAuthor(int userId, int categoryId)
    {
        var wuwi = WuwiQuestionsFromCategory(userId, categoryId); 

        return UserIsQuestionAuthor(userId, categoryId).Concat(wuwi).ToList(); 
    }


    private static List<Question> NotWuwiFromCategoryOrIsAuthor(int userId, int categoryId)
    {
        var isNotWuwi = NotWuwiFromCategory(userId, categoryId);

        return UserIsQuestionAuthor(userId, categoryId).Concat(isNotWuwi).ToList(); 
    }

    private static List<Question> IsInWuWiFromCategoryAndIsNotInWuwi(int userId, int categoryId)
    {
          
        return GetCategoryQuestionsFromEntityCache(categoryId).Where(q => q.Creator.Id != userId).ToList();
    }

    private static List<Question> NotWuwiFromCategory(int userId, int categoryId)
    {
        return CompareDictionaryWithQuestionsFromMemoryCache(GetIdsFromQuestionValuation (userId), categoryId, true);
    }

    private static List<Question> UserIsQuestionAuthor(int userId, int categoryId)
    {
        return EntityCache.GetCategory(categoryId)
            .GetAggregatedQuestionsFromMemoryCache().Where(q => q.Creator.Id == userId).ToList();
    }

    public static List<Question> GetCategoryQuestionsFromEntityCache(int categoryId)
    {
        return EntityCache.GetCategory(categoryId).GetAggregatedQuestionsFromMemoryCache().ToList();
    }

    private static IList<Question> OrderByProbability( List<Question> questions)
    {
        return questions.OrderByDescending(q => q.CorrectnessProbability).ToList();
    }

    private static List<Question> CompareDictionaryWithQuestionsFromMemoryCache(Dictionary<int, int> dic1, int categoryId, bool isNotWuwi = false)
    {
        List<Question> questions = new List<Question>();
        var questionsFromEntityCache = EntityCache.GetCategory(categoryId)
            .GetAggregatedQuestionsFromMemoryCache().ToDictionary(q => q.Id);

        if (!isNotWuwi)
        {
            foreach (var q in questionsFromEntityCache)
            {
                if (dic1.ContainsKey(q.Key))
                    questions.Add(q.Value);
            }
        }
        else
        {
            foreach (var qId in dic1)
            {
                questionsFromEntityCache.Remove(qId.Key);
                questions = questionsFromEntityCache.Values.ToList(); 
            }
        }
        
        return questions;
    }

    private static Dictionary<int, int> GetIdsFromQuestionValuation(int userId)
    {
       return UserCache.GetQuestionValuations(userId).Where(qv => qv.IsInWishKnowledge)
            .Select(qv => qv.Question.Id).ToDictionary(q => q);
    }

    private static List<Question> GetQuestionsFromMinToMaxProbability(int minProbability, int maxProbability, List<Question> questions)
    {
        var result = questions.Where(q =>
            q.CorrectnessProbability >= minProbability && q.CorrectnessProbability <= maxProbability);
        return result.ToList(); 
    }
}
