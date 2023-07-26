﻿using Seedworks.Web.State;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Serilog;

public class SessionUserCache
{
    public const int ExpirationSpanInMinutes = 600;

    private const string SessionUserCacheItemPrefix = "SessionUserCacheItem_";
    private static string GetCacheKey(int userId) => SessionUserCacheItemPrefix + userId;

    public static List<SessionUserCacheItem> GetAllCacheItems(CategoryValuationReadingRepo categoryValuationReadingRepo, UserReadingRepo userReadingRepo, QuestionValuationRepo questionValuationRepo) //todo: Wir haben zum abgleichen des Caches einen Job, den benötigen wir nicht wenn wir manuell abgleichen
    {
        var allUserIds = userReadingRepo.GetAllIds();
        return allUserIds.Select(uId=> GetItem(uId, categoryValuationReadingRepo, userReadingRepo, questionValuationRepo)).ToList();
    }

    public static SessionUserCacheItem GetUser(int userId, CategoryValuationReadingRepo categoryValuationReadingRepo, UserReadingRepo userReadingRepo, QuestionValuationRepo questionValuationRepo) =>
        GetItem(userId, categoryValuationReadingRepo, userReadingRepo, questionValuationRepo);

    private static readonly string _createItemLockKey = "2FB5BC59-9E90-4511-809A-BC67A6D35F7F";
    public static SessionUserCacheItem GetItem(int userId, CategoryValuationReadingRepo categoryValuationReadingRepo, UserReadingRepo userReadingRepo, QuestionValuationRepo questionValuationRepo)
    {
        var cacheItem = Cache.Get<SessionUserCacheItem>(GetCacheKey(userId));
        if (cacheItem != null)
            return cacheItem;

        lock (_createItemLockKey)                                                            
        {
            //recheck if the cache item exists
            Log.Information("GetUserCacheItem: {userId}", userId);
            cacheItem = Cache.Get<SessionUserCacheItem>(GetCacheKey(userId));
            return cacheItem ?? CreateItemFromDatabase(userId, categoryValuationReadingRepo, userReadingRepo, questionValuationRepo);
        }
    }

    public static bool IsInWishknowledge(int userId, int categoryId, CategoryValuationReadingRepo categoryValuationReadingRepo, UserReadingRepo userReadingRepo, QuestionValuationRepo questionValuationRepo)
    {
        var cacheItem = GetItem(userId, categoryValuationReadingRepo,userReadingRepo, questionValuationRepo);
        var hasCategoryValuation = cacheItem.CategoryValuations.ContainsKey(categoryId);

        if (!hasCategoryValuation)
            return false;

        return cacheItem.CategoryValuations[categoryId].IsInWishKnowledge();
    }

    public static bool IsQuestionInWishknowledge(int userId, int questionId, CategoryValuationReadingRepo categoryValuationReadingRepo, UserReadingRepo userReadingRepo, QuestionValuationRepo questionValuationRepo)
    {
        var cacheItem = GetItem(userId, categoryValuationReadingRepo, userReadingRepo, questionValuationRepo);
        var hasQuestionValuation = cacheItem.QuestionValuations.ContainsKey(questionId);

        if (!hasQuestionValuation)
            return false;

        return cacheItem.QuestionValuations[questionId].IsInWishKnowledge;
    }

    public static SessionUserCacheItem CreateItemFromDatabase(int userId, 
        CategoryValuationReadingRepo categoryValuationReadingRepo, 
        UserReadingRepo userReadingRepo,
        QuestionValuationRepo questionValuationRepo)
    {
        var user = userReadingRepo.GetById(userId);

        if (user == null) return null;

        var cacheItem = SessionUserCacheItem.CreateCacheItem(user);
        cacheItem.CategoryValuations = new ConcurrentDictionary<int, CategoryValuation>(
            categoryValuationReadingRepo.GetByUser(userId, onlyActiveKnowledge: false)
                .Select(v => new KeyValuePair<int, CategoryValuation>(v.CategoryId, v)));

        Add_UserCacheItem_to_cache(cacheItem, userId);

        var addedCacheItem = Cache.Get<SessionUserCacheItem>(GetCacheKey(userId));

        addedCacheItem.QuestionValuations = new ConcurrentDictionary<int, QuestionValuationCacheItem>(
            questionValuationRepo.GetByUserWithQuestion(userId)
                .Select(v =>
                    new KeyValuePair<int, QuestionValuationCacheItem>(v.Question.Id,
                        QuestionValuationCacheItem.ToCacheItem(v))));

        return addedCacheItem;
    }

    private static void Add_UserCacheItem_to_cache(SessionUserCacheItem cacheItem, int userId)
    {
        Cache.Add(GetCacheKey(userId), cacheItem, TimeSpan.FromMinutes(ExpirationSpanInMinutes),
            slidingExpiration: true);
    }

    public static IList<QuestionValuationCacheItem> GetQuestionValuations(int userId, CategoryValuationReadingRepo categoryValuationReadingRepo, UserReadingRepo userReadingRepo, QuestionValuationRepo questionValuationRepo) =>
        GetItem(userId, categoryValuationReadingRepo, userReadingRepo, questionValuationRepo)?.QuestionValuations.Values.ToList() ?? new List<QuestionValuationCacheItem>();

    public static IList<CategoryValuation> GetCategoryValuations(int userId, CategoryValuationReadingRepo categoryValuationReadingRepo, UserReadingRepo userReadingRepo, QuestionValuationRepo questionValuationRepo) =>
        GetItem(userId, categoryValuationReadingRepo, userReadingRepo, questionValuationRepo).CategoryValuations.Values.ToList();

    public static void AddOrUpdate(QuestionValuationCacheItem questionValuation, CategoryValuationReadingRepo categoryValuationReadingRepo, UserReadingRepo userReadingRepo, QuestionValuationRepo questionValuationRepo)
    {
        var cacheItem = GetItem(questionValuation.User.Id, categoryValuationReadingRepo, userReadingRepo, questionValuationRepo);

        lock ("7187a2c9-a3a2-42ca-8202-f9cb8cb54137")
        {
            cacheItem.QuestionValuations.AddOrUpdate(questionValuation.Question.Id, questionValuation,
                (k, v) => questionValuation);
        }
    }

    public static void AddOrUpdate(CategoryValuation categoryValuation, CategoryValuationReadingRepo categoryValuationReadingRepo, UserReadingRepo userReadingRepo, QuestionValuationRepo questionValuationRepo)
    {
        var cacheItem = GetItem(categoryValuation.UserId, categoryValuationReadingRepo, userReadingRepo, questionValuationRepo);

        lock ("82f573db-40a7-43d9-9e68-6cd78b626e8d")
        {
            cacheItem.CategoryValuations.AddOrUpdate(categoryValuation.CategoryId, categoryValuation,
                (k, v) => categoryValuation);
        }
    }

    public static void AddOrUpdate(User user, CategoryValuationReadingRepo categoryValuationReadingRepo, UserReadingRepo userReadingRepo, QuestionValuationRepo questionValuationRepo)
    {
        var cacheItem = GetItem(user.Id, categoryValuationReadingRepo, userReadingRepo, questionValuationRepo);
        cacheItem.AssignValues(user);
    }

    /// <summary> Used for question delete </summary>
    public static void RemoveQuestionForAllUsers(int questionId,
        CategoryValuationReadingRepo categoryValuationReadingRepo,
        UserReadingRepo userReadingRepo,
        QuestionValuationRepo questionValuationRepo)
    {
        foreach (var userId in userReadingRepo.GetAllIds())
        {
            RemoveQuestionValuationForUser(userId, questionId, categoryValuationReadingRepo, userReadingRepo, questionValuationRepo);
        }
    }

    public static void RemoveQuestionValuationForUser(int userId, int questionId, CategoryValuationReadingRepo categoryValuationReadingRepo, UserReadingRepo userReadingRepo, QuestionValuationRepo questionValuationRepo)
    {
        var cacheItem = GetItem(userId, categoryValuationReadingRepo, userReadingRepo, questionValuationRepo);
        cacheItem?.QuestionValuations.TryRemove(questionId, out _);
    }

    public static List<SessionUserCacheItem> GetAllActiveCaches(CategoryValuationReadingRepo categoryValuationReadingRepo, UserReadingRepo userReadingRepo, QuestionValuationRepo questionValuationRepo)
    {
        var enumerator = System.Web.HttpRuntime.Cache.GetEnumerator();
        List<string> keys = new List<string>();
        List<SessionUserCacheItem> userCacheItems = new List<SessionUserCacheItem>();

        while (enumerator.MoveNext())
        {
            keys.Add(enumerator.Key.ToString());
        }

        foreach (var userCacheKey in keys.Where(k => k.Contains(SessionUserCacheItemPrefix)))
        {
            var startIndex = userCacheKey.IndexOf("_") + 1;
            var endIndex = userCacheKey.Length - startIndex;
            var userId = Int32.Parse(userCacheKey.Substring(startIndex, endIndex));
            userCacheItems.Add(GetItem(userId, categoryValuationReadingRepo, userReadingRepo, questionValuationRepo));
        }

        return userCacheItems;
    }

    /// <summary> Used for category delete </summary>
    public static void RemoveAllForCategory(int categoryId, 
        CategoryValuationReadingRepo categoryValuationReadingRepo,
        CategoryValuationWritingRepo categoryValuationWritingRepo,
        UserReadingRepo userReadingRepo, 
        QuestionValuationRepo questionValuationRepo)
    {
        categoryValuationWritingRepo.DeleteCategoryValuation(categoryId);
        foreach (var userCache in GetAllActiveCaches(categoryValuationReadingRepo, userReadingRepo, questionValuationRepo))
        {
            userCache.CategoryValuations.TryRemove(categoryId, out var result);
        }
    }

    public static void Remove(User user) => Remove(user.Id);

    public static void Remove(int userId)
    {
        var cacheKey = GetCacheKey(userId);
        var cacheItem = Cache.Get<SessionUserCacheItem>(cacheKey);

        if (cacheItem != null)
        {
            Cache.Remove(cacheKey);
        }
    }
}