using Seedworks.Web.State;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

public class UserCache
{
    public const int ExpirationSpanInMinutes = 600;
    private static string GetCacheKey(int userId) => "UserCashItem_" + userId;
    public static bool IsFiltered = false; 

    public static List<UserCacheItem> GetAllCacheItems()
    {
        var allUserIds = Sl.UserRepo.GetAllIds();
        var allUserValuations = new List<UserCacheItem>();
        foreach (var userId in allUserIds)                                  
        {
            allUserValuations.Add(GetItem(userId));
        }

        return allUserValuations;
    }
    public static UserCacheItem GetItem(int userId)
    {
        var cacheItem = Cache.Get<UserCacheItem>(GetCacheKey(userId));
        return cacheItem ?? CreateItemFromDatabase(userId);
    }

    public static UserCacheItem CreateItemFromDatabase(int userId)
    {
        var user = Sl.UserRepo.GetById(userId);

        var cacheItem = new UserCacheItem
        {
            User = user,
            CategoryValuations = new ConcurrentDictionary<int, CategoryValuation>(
                Sl.CategoryValuationRepo.GetByUser(userId, onlyActiveKnowledge: false)
                    .Select(v => new KeyValuePair<int, CategoryValuation>(v.CategoryId, v))),
            QuestionValuations = new ConcurrentDictionary<int, QuestionValuation>(
                Sl.QuestionValuationRepo.GetByUserWithQuestion(userId)
                    .Select(v => new KeyValuePair<int, QuestionValuation>(v.Question.Id, v))),
            IsFiltered = IsFiltered
        };
            
        Add_valuationCacheItem_to_cache(cacheItem, userId);

        return cacheItem;
    }

    private static void Add_valuationCacheItem_to_cache(UserCacheItem cacheItem, int userId)
    {
        Cache.Add(GetCacheKey(userId), cacheItem, TimeSpan.FromMinutes(ExpirationSpanInMinutes), slidingExpiration: true);
    }

    public static IList<QuestionValuation> GetQuestionValuations(int userId) => GetItem(userId).QuestionValuations.Values.ToList();
    public static IList<CategoryValuation> GetCategoryValuations(int userId) => GetItem(userId).CategoryValuations.Values.ToList();

    public static void AddOrUpdate(QuestionValuation questionValuation)
    {
        var cacheItem = GetItem(questionValuation.User.Id);

        lock ("7187a2c9-a3a2-42ca-8202-f9cb8cb54137")
        {
            cacheItem.QuestionValuations.AddOrUpdate(questionValuation.Question.Id, questionValuation, (k, v) => questionValuation);
        }
    }

    public static void AddOrUpdate(CategoryValuation categoryValuation)
    {
        var cacheItem = GetItem(categoryValuation.UserId);

        lock ("82f573db-40a7-43d9-9e68-6cd78b626e8d")
        {
            cacheItem.CategoryValuations.AddOrUpdate(categoryValuation.CategoryId, categoryValuation, (k, v) => categoryValuation);
        }
    }

    public static void AddOrUpdate(User user)
    {
        var cacheItem = GetItem(user.Id);
        cacheItem.User = user;
    }

    /// <summary> Used for question delete </summary>
    public static void RemoveAllForQuestion(int questionId)
    {
        foreach (var userId in Sl.UserRepo.GetAllIds())
        {
            var cacheItem = GetItem(userId);
            cacheItem.QuestionValuations.TryRemove(questionId, out var questValOut);
        }
    }

    /// <summary> Used for category delete </summary>
    public static void RemoveAllForCategory(int categoryId)
    {
        foreach (var userId in Sl.UserRepo.GetAllIds())
        {
            var cacheItem = GetItem(userId);
            cacheItem.CategoryValuations.TryRemove(categoryId, out var catValOut);
        }
    }

    public static void Remove(User user)
    {
        var cacheKey = GetCacheKey(user.Id);
        var cacheItem = Cache.Get<UserCacheItem>(cacheKey);

        if (cacheItem != null)
        {
            Cache.Remove(cacheKey);
        }
    }
}
