﻿using System.Collections.Concurrent;
using ConcurrentCollections;

public class SessionUserCache(
    CategoryValuationReadingRepo _categoryValuationReadingRepo,
    QuestionValuationReadingRepo _questionValuationReadingRepo)
    : IRegisterAsInstancePerLifetime
{
    private readonly object cacheLock = new();
    public const int ExpirationSpanInMinutes = 600;
    private const string SessionUserCacheItemPrefix = "SessionUserCacheItem_";

    private string GetCacheKey(int userId) => SessionUserCacheItemPrefix + userId;

    public List<SessionUserCacheItem?> GetAllCacheItems()
    {
        return EntityCache.GetAllUsers()
            .Select(user => GetItem(user.Id))
            .Where(sessionUserCacheItem => sessionUserCacheItem != null)
            .ToList();
    }

    public SessionUserCacheItem? GetUser(int userId) =>
        GetItem(userId);

    public SessionUserCacheItem? GetItem(int userId) =>
        Seedworks.Web.State.Cache.Get<SessionUserCacheItem>(GetCacheKey(userId));

    public bool ItemExists(int userId)
    {
        return Seedworks.Web.State.Cache.Contains(GetCacheKey(userId));
    }

    public bool IsQuestionInWishknowledge(int userId, int questionId)
    {
        var cacheItem = GetItem(userId);

        var hasQuestionValuation = cacheItem.QuestionValuations.ContainsKey(questionId);

        if (!hasQuestionValuation)
            return false;

        return cacheItem.QuestionValuations[questionId].IsInWishKnowledge;
    }

    public IList<QuestionValuationCacheItem> GetQuestionValuations(int userId) =>
        GetItem(userId)?.QuestionValuations.Values
            .ToList() ?? new List<QuestionValuationCacheItem>();

    public IList<CategoryValuation> GetCategoryValuations(int userId)
    {
        var item = GetItem(userId);

        if (item != null)
        {
            return item.CategoryValuations.Values.ToList();
        }

        Logg.r.Error("sessionUserItem is null");
        return new List<CategoryValuation>();
    }

    public void AddOrUpdate(QuestionValuationCacheItem questionValuation)
    {
        var cacheItem = GetItem(questionValuation.User.Id);

        lock (cacheLock)
        {
            cacheItem.QuestionValuations.AddOrUpdate(questionValuation.Question.Id,
                questionValuation,
                (k, v) => questionValuation);
        }
    }

    private readonly object _addOrUpdateLock = new();

    public void AddOrUpdate(User user)
    {
        lock (_addOrUpdateLock)
        {
            var cacheItem = GetItem(user.Id);
            if (cacheItem == null)
            {
                cacheItem = CreateSessionUserItemFromDatabase(user);
                cacheItem.AssignValues(user);
                AddToCache(cacheItem);
            }

            Remove(user);
            cacheItem.AssignValues(user);
            AddToCache(cacheItem);
        }
    }

    public void RemoveQuestionForAllUsers(int questionId)
    {
        foreach (var user in EntityCache.GetAllUsers())
            RemoveQuestionValuationForUser(user.Id, questionId);
    }

    public void RemoveQuestionValuationForUser(int userId, int questionId)
    {
        if (ItemExists(userId))
        {
            var cacheItem = GetItem(userId);
            cacheItem?.QuestionValuations.TryRemove(questionId, out _);
        }
    }

    /// <summary>
    /// Get all active UserCaches
    /// </summary>
    /// <returns></returns>
    public List<SessionUserCacheItem> GetAllActiveCaches()
    {
        var allUsers = EntityCache.GetAllUsers();
        var userCacheItems = allUsers
            .Select(user => GetItem(user.Id))
            .Where(item => item != null)
            .ToList();

        return userCacheItems;
    }

    /// <summary> Used for category delete </summary>
    public void RemoveAllForCategory(
        int categoryId,
        CategoryValuationWritingRepo categoryValuationWritingRepo)
    {
        categoryValuationWritingRepo.DeleteCategoryValuation(categoryId);
        foreach (var userCache in GetAllActiveCaches())
        {
            userCache.CategoryValuations.TryRemove(categoryId, out var result);
        }
    }

    public void Remove(User user) => Remove(user.Id);

    public void Remove(int userId)
    {
        var cacheKey = GetCacheKey(userId);
        var cacheItem = Seedworks.Web.State.Cache.Get<SessionUserCacheItem>(cacheKey);

        if (cacheItem != null)
        {
            Seedworks.Web.State.Cache.Remove(cacheKey);
        }
    }

    public SessionUserCacheItem CreateSessionUserItemFromDatabase(User? user)
    {
        if (user == null)
            return null;

        var cacheItem = SessionUserCacheItem.CreateCacheItem(user);

        cacheItem.CategoryValuations = new ConcurrentDictionary<int, CategoryValuation>(
            _categoryValuationReadingRepo.GetByUser(user.Id, onlyActiveKnowledge: false)
                .Select(v => new KeyValuePair<int, CategoryValuation>(v.CategoryId, v)));

        cacheItem.QuestionValuations = new ConcurrentDictionary<int, QuestionValuationCacheItem>(
            _questionValuationReadingRepo.GetByUserWithQuestion(user.Id)
                .Select(v =>
                    new KeyValuePair<int, QuestionValuationCacheItem>(v.Question.Id,
                        QuestionValuationCacheItem.ToCacheItem(v))));

        return cacheItem;
    }

    private void AddToCache(SessionUserCacheItem cacheItem)
    {
        Seedworks.Web.State.Cache.Add(GetCacheKey(cacheItem.Id), cacheItem,
            TimeSpan.FromMinutes(ExpirationSpanInMinutes),
            slidingExpiration: true);
    }
}