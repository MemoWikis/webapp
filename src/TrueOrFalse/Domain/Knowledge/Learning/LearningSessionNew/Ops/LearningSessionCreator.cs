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

        if (config.AllQuestions  && !UserCache.GetItem(config.CurrentUserId).IsFiltered)
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
        else if (config.InWishknowledge && !config.CreatedByCurrentUser)
            questions = OrderByProbability(
                RandomLimited(WuwiAndNotAuthor(config.CurrentUserId, config.CategoryId), config)).ToList();
        else if (config.InWishknowledge)
            questions = OrderByProbability(
                RandomLimited(WuwiQuestionsFromCategory(config.CurrentUserId, config.CategoryId).ToList(), config)).ToList();
        else if (config.CreatedByCurrentUser)
            questions = OrderByProbability(
                RandomLimited(UserIsQuestionAuthor(config.CurrentUserId, config.CategoryId), config)).ToList();

        return new LearningSession(questions.Distinct().Select(q => new LearningSessionStep(q)).ToList(), config);
    }

    public static int GetQuestionCount(LearningSessionConfig config)
    {
        config.MaxQuestionCount = 0;
        return ForLoggedInUser(config).Steps.Count; 
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

    private static IList<Question> WuwiQuestionsFromCategory(int userId, int categoryId)
    {
        return CompareDictionaryWithQuestionsFromMemoryCache(GetIdsFromQuestionValuation(userId), categoryId);
    }

    private static List<Question> WuwiQuestionsFromCategoryAndUserIsAuthor(int userId, int categoryId)
    {
        return UserIsQuestionAuthor(userId, categoryId)
            .Concat(WuwiQuestionsFromCategory(userId, categoryId))
            .ToList();
    }


    private static List<Question> NotWuwiFromCategoryOrIsAuthor(int userId, int categoryId)
    {
        var isNotWuwi = NotWuwiFromCategory(userId, categoryId);
        return UserIsQuestionAuthor(userId, categoryId, true).Concat(isNotWuwi).ToList(); 
    }

    private static List<Question> WuwiAndNotAuthor(int userId, int categoryId)
    {
        var wuwi = WuwiQuestionsFromCategory(userId, categoryId);
        return wuwi.Where(q => new UserTinyModel(q.Creator).Id != userId).ToList();
    }

    private static List<Question> IsInWuWiFromCategoryAndIsNotInWuwi(int userId, int categoryId)
    {
        return GetCategoryQuestionsFromEntityCache(categoryId).Where(q =>
            new UserTinyModel(q.Creator).Id != userId)
            .ToList();
    }

    private static List<Question> NotWuwiFromCategory(int userId, int categoryId)
    {
        return CompareDictionaryWithQuestionsFromMemoryCache(GetIdsFromQuestionValuation (userId), categoryId, true);
    }

    private static List<Question> UserIsQuestionAuthor(int userId, int categoryId, bool isNotInWuwi = false)
    {
        if (isNotInWuwi)
            return EntityCache.GetCategoryCacheItem(categoryId)
                .GetAggregatedQuestionsFromMemoryCache().Where(q =>
                    new UserTinyModel(q.Creator).Id == userId && !q.IsInWishknowledge())
                .ToList();

        if (UserCache.GetItem(Sl.CurrentUserId).IsFiltered)
            return EntityCache.GetCategoryCacheItem(categoryId)
                .GetAggregatedQuestionsFromMemoryCache().Where(q =>
                    new UserTinyModel(q.Creator).Id == userId && q.IsInWishknowledge())
                .ToList(); 

        return EntityCache.GetCategoryCacheItem(categoryId)
            .GetAggregatedQuestionsFromMemoryCache().Where(q =>
                 new UserTinyModel(q.Creator).Id  == userId)
            .ToList();
    }

    public static List<Question> GetCategoryQuestionsFromEntityCache(int categoryId)
    {
        return EntityCache.GetCategoryCacheItem(categoryId).GetAggregatedQuestionsFromMemoryCache().ToList();
    }

    private static IList<Question> OrderByProbability( List<Question> questions)
    {
        return questions.OrderByDescending(q => q.CorrectnessProbability).ToList();
    }

    private static List<Question> CompareDictionaryWithQuestionsFromMemoryCache(Dictionary<int, int> dic1, int categoryId, bool isNotWuwi = false)
    {
        List<Question> questions = new List<Question>();
        var questionsFromEntityCache = EntityCache.GetCategoryCacheItem(categoryId)
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

    public static LearningSession BuildLearningSession(LearningSessionConfig config)
    {
        IList<Question> questions = new List<Question>();
        var allQuestions = EntityCache.GetCategoryCacheItem(config.CategoryId).GetAggregatedQuestionsFromMemoryCache(onlyVisible: true);

        questions = FilterByWuwi(config, allQuestions);
        questions = FilterByCreator(config, questions);
        questions = FilterByVisibility(config, questions);
        questions = FilterByKnowledgeSummary(config, questions);

        return new LearningSession(questions.Distinct().Select(q => new LearningSessionStep(q)).ToList(), config);
    }

    private static IList<Question> FilterByWuwi(LearningSessionConfig config, IList<Question> allQuestions)
    {
        if (!config.InWuwi && config.NotInWuwi)
            return NotWuwiFromCategory(Sl.SessionUser.UserId, config.CategoryId);

        if (config.InWuwi && !config.NotInWuwi)
            return WuwiQuestionsFromCategory(Sl.SessionUser.UserId, config.CategoryId);

        return allQuestions;
    }

    private static IList<Question> FilterByCreator(LearningSessionConfig config, IList<Question> questions)
    {
        if (config.CreatedByCurrentUser && !config.NotCreatedByCurrentUser)
            return questions.Where(q => q.Creator.Id == Sl.SessionUser.User.Id).ToList();

        if (!config.CreatedByCurrentUser && config.NotCreatedByCurrentUser)
            return questions.Where(q => q.Creator.Id != Sl.SessionUser.User.Id).ToList();

        return questions;
    }

    private static IList<Question> FilterByVisibility(LearningSessionConfig config, IList<Question> questions)
    {
        if (config.PrivateQuestions && !config.PublicQuestions)
            return questions.Where(q => q.Visibility == QuestionVisibility.Owner).ToList();

        if (!config.PrivateQuestions && config.PublicQuestions)
            return questions.Where(q => q.Visibility == QuestionVisibility.All).ToList();

        return questions;
    }

    private static IList<Question> FilterByKnowledgeSummary(LearningSessionConfig config, IList<Question> questions)
    {
        bool notLearned = config.NotLearned;
        bool needsLearning = config.NeedsLearning;
        bool needsConsolidation = config.NeedsConsolidation;
        bool solid = config.Solid;

        var filteredQuestions = new List<Question>();

        if (notLearned && needsLearning && needsConsolidation && solid)
            return questions;

        var allQuestionValuation = Sl.QuestionValuationRepo.GetByUser(Sl.SessionUser.UserId);

        if (notLearned)
        {
            var unansweredQuestions = questions.Where(q => allQuestionValuation.All(v => v.Id != q.Id));

            filteredQuestions.AddRange(
                questions.Where(q =>
                    unansweredQuestions.Any(v => v.Id == q.Id)));
        }

        if (needsLearning)
        {
            var questionsNeedLearning = allQuestionValuation.Where(q =>
                q.CorrectnessProbability > 50 &&
                q.CorrectnessProbability < 50);

            filteredQuestions.AddRange(
                questions.Where(q =>
                    questionsNeedLearning.Any(v => v.Id == q.Id)));
        }

        if (needsConsolidation)
        {
            var questionsNeedsConsolidation = allQuestionValuation.Where(q =>
                q.CorrectnessProbability >= 50 &&
                q.CorrectnessProbability < 80);

            filteredQuestions.AddRange(
                questions.Where(q =>
                    questionsNeedsConsolidation.Any(v => v.Id == q.Id)));
        }

        if (solid)
        {
            var solidQuestions = allQuestionValuation.Where(q =>
                q.CorrectnessProbability >= 80);

            filteredQuestions.AddRange(
                questions.Where(q =>
                    solidQuestions.Any(v => v.Id == q.Id)));
        }

        return filteredQuestions;
    }
}
