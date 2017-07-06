using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Seedworks.Web.State;

public class UserValuationCache
{
    public const int ExpirationSpanInMinutes = 60;

    public static string GetCacheKey(int userId) => "UserValuationCacheItem_" + userId;

    public static UserValuationCacheItem GetItem(int userId)
    {
        var cacheItem = Cache.Get<UserValuationCacheItem>(GetCacheKey(userId));
        return cacheItem ?? CreateItemFromDatabase(userId);
    }

    private static UserValuationCacheItem CreateItemFromDatabase(int userId)
    {
        var cacheItem = new UserValuationCacheItem
        {
            UserId = userId,
            CategoryValuations = new ConcurrentDictionary<int, CategoryValuation>(
                Sl.CategoryValuationRepo.GetByUser(userId, onlyActiveKnowledge: false)
                .Select(v => new KeyValuePair<int, CategoryValuation>(v.CategoryId, v))),
            QuestionValuations = new ConcurrentDictionary<int, QuestionValuation>(
                Sl.QuestionValuationRepo.GetByUser(userId, onlyActiveKnowledge: false)
                .Select(v => new KeyValuePair<int, QuestionValuation>(v.Question.Id, v)))
        };
        Add_valuationCacheItem_to_cache(cacheItem, userId);

        return cacheItem;
    }

    private static void Add_valuationCacheItem_to_cache(UserValuationCacheItem cacheItem, int userId)
    {
        Cache.Add(GetCacheKey(userId), cacheItem, TimeSpan.FromMinutes(ExpirationSpanInMinutes), slidingExpiration: true);
    }

    public static IList<QuestionValuation> GetQuestionValuations(int userId) => GetItem(userId).QuestionValuations.Values.ToList();

    public static void AddOrUpdate(QuestionValuation questionValuation)
    {
        var cacheItem = GetItem(questionValuation.User.Id);

        lock ("7187a2c9-a3a2-42ca-8202-f9cb8cb54137")
        {
            cacheItem.QuestionValuations.AddOrUpdate(questionValuation.Question.Id, questionValuation, (k, v) => v);
        }
    }

    public static void Remove(QuestionValuation questionValuation)
    {
        var cacheItem = GetItem(questionValuation.User.Id);

        cacheItem.QuestionValuations.TryRemove(questionValuation.Question.Id, out var questionValOut);
    }

    public static void AddOrUpdate(CategoryValuation categoryValuation)
    {
        var cacheItem = GetItem(categoryValuation.UserId);

        lock ("82f573db-40a7-43d9-9e68-6cd78b626e8d")
        {
            cacheItem.CategoryValuations.AddOrUpdate(categoryValuation.CategoryId, categoryValuation, (k, v) => v);
        }
    }

    public static void RemoveAllForQuestion(int questionId)
    {
        foreach (var userId in Sl.UserRepo.GetAllIds())
        {
            var cacheItem = GetItem(userId);
            cacheItem.QuestionValuations.TryRemove(questionId, out var questValOut);
        }
    }

    public static void RemoveAllForCategory(int categoryId)
    {
        foreach (var userId in Sl.UserRepo.GetAllIds())
        {
            var cacheItem = GetItem(userId);
            cacheItem.CategoryValuations.TryRemove(categoryId, out var catValOut);
        }
        
    }
}
