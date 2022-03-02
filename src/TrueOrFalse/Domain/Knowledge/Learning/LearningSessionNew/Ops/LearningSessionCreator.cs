﻿using System.Collections.Generic;
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
        List<QuestionCacheItem> questions = new List<QuestionCacheItem>();
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
                RandomLimited(WuwiQuestionsFromCategory(config.CurrentUserId, config.CategoryId), config)).ToList();
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

    private static List<QuestionCacheItem> RandomLimited(List<QuestionCacheItem> questions, LearningSessionConfig config)
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

    private static List<QuestionCacheItem> WuwiQuestionsFromCategory(int userId, int categoryId)
    {
        return CompareDictionaryWithQuestionsFromMemoryCache(GetIdsFromQuestionValuation(userId), categoryId);
    }

    private static List<QuestionCacheItem> WuwiQuestionsFromCategoryAndUserIsAuthor(int userId, int categoryId)
    {
        return UserIsQuestionAuthor(userId, categoryId)
            .Concat(WuwiQuestionsFromCategory(userId, categoryId))
            .ToList();
    }


    private static List<QuestionCacheItem> NotWuwiFromCategoryOrIsAuthor(int userId, int categoryId)
    {
        var isNotWuwi = NotWuwiFromCategory(userId, categoryId);
        return UserIsQuestionAuthor(userId, categoryId, true).Concat(isNotWuwi).ToList(); 
    }

    private static List<QuestionCacheItem> WuwiAndNotAuthor(int userId, int categoryId)
    {
        var wuwi = WuwiQuestionsFromCategory(userId, categoryId);
        return wuwi.Where(q => new UserTinyModel(q.Creator).Id != userId).ToList();
    }

    private static List<QuestionCacheItem> IsInWuWiFromCategoryAndIsNotInWuwi(int userId, int categoryId)
    {
        return GetCategoryQuestionsFromEntityCache(categoryId).Where(q =>
            new UserTinyModel(q.Creator).Id != userId)
            .ToList();
    }

    private static List<QuestionCacheItem> NotWuwiFromCategory(int userId, int categoryId)
    {
        return CompareDictionaryWithQuestionsFromMemoryCache(GetIdsFromQuestionValuation (userId), categoryId, true);
    }

    private static List<QuestionCacheItem> UserIsQuestionAuthor(int userId, int categoryId, bool isNotInWuwi = false)
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

    public static List<QuestionCacheItem> GetCategoryQuestionsFromEntityCache(int categoryId)
    {
        return EntityCache.GetCategoryCacheItem(categoryId).GetAggregatedQuestionsFromMemoryCache().ToList();
    }

    private static IList<QuestionCacheItem> OrderByProbability( List<QuestionCacheItem> questions)
    {
        return questions.OrderByDescending(q => q.CorrectnessProbability).ToList();
    }

    private static List<QuestionCacheItem> CompareDictionaryWithQuestionsFromMemoryCache(Dictionary<int, int> dic1, int categoryId, bool isNotWuwi = false)
    {
        List<QuestionCacheItem> questions = new List<QuestionCacheItem>();
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

    private static List<QuestionCacheItem> GetQuestionsFromMinToMaxProbability(int minProbability, int maxProbability, List<QuestionCacheItem> questions)
    {
        var result = questions.Where(q =>
            q.CorrectnessProbability >= minProbability && q.CorrectnessProbability <= maxProbability);
        return result.ToList(); 
    }
}
