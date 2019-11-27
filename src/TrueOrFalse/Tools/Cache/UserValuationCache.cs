using Seedworks.Web.State;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

public class UserValuationCache
{
    public const int ExpirationSpanInMinutes = 600;

    public static string GetCacheKey(int userId) => "UserValuationCacheItem_" + userId;

    public static UserValuationCacheItem GetItem(int userId)
    {
        var cacheItem = Cache.Get<UserValuationCacheItem>(GetCacheKey(userId));
        return cacheItem ?? CreateItemFromDatabase(userId);
    }

    public static List<UserValuationCacheItem> GetAllCacheItems()
    {
        var allUserIds = Sl.UserRepo.GetAllIds();
        var allUserValuations = new List<UserValuationCacheItem>();
        foreach (var userId in allUserIds)                                  
        {
            allUserValuations.Add(GetItem(userId));
        }

        return allUserValuations;
    }

    public static UserValuationCacheItem CreateItemFromDatabase(int userId)
    {
        var cacheItem = new UserValuationCacheItem
        {
            UserId = userId,
            CategoryValuations = new ConcurrentDictionary<int, CategoryValuation>(
                Sl.CategoryValuationRepo.GetByUser(userId, onlyActiveKnowledge: false)
                .Select(v => new KeyValuePair<int, CategoryValuation>(v.CategoryId, v))),
            QuestionValuations = new ConcurrentDictionary<int, QuestionValuation>(
                Sl.QuestionValuationRepo.GetByUserWithQuestion(userId)
                .Select(v => new KeyValuePair<int, QuestionValuation>(v.Question.Id, v))),
            SetValuations = new ConcurrentDictionary<int, SetValuation>(
                Sl.SetValuationRepo.GetByUser(userId, onlyActiveKnowledge: false)
                .Select(v => new KeyValuePair<int, SetValuation>(v.SetId, v)))
        };
        Add_valuationCacheItem_to_cache(cacheItem, userId);

        return cacheItem;
    }

    private static void Add_valuationCacheItem_to_cache(UserValuationCacheItem cacheItem, int userId)
    {
        Cache.Add(GetCacheKey(userId), cacheItem, TimeSpan.FromMinutes(ExpirationSpanInMinutes), slidingExpiration: true);
    }

    public static IList<QuestionValuation> GetQuestionValuations(int userId) => GetItem(userId).QuestionValuations.Values.ToList();
    public static IList<SetValuation> GetSetValuations(int userId) => GetItem(userId).SetValuations.Values.ToList();
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

    public static void AddOrUpdate(SetValuation setValuation)
    {
        var cacheItem = GetItem(setValuation.UserId);

        lock ("76g99f0r-784h-222f-778g-kl7755889h7d")
        {
            cacheItem.SetValuations.AddOrUpdate(setValuation.SetId, setValuation, (k, v) => setValuation);
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

    public static void RemoveAllForSet(int setId)
    {
        foreach (var userId in Sl.UserRepo.GetAllIds())
        {
            var cacheItem = GetItem(userId);
            cacheItem.SetValuations.TryRemove(setId, out var setValOut);
        }
    }
}
