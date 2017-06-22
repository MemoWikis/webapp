using System;
using System.Collections.Generic;
using System.Threading;
using Seedworks.Web.State;


public class UserValuationCache
{
    public const int ExpirationSpanInMinutes = 60;

    public static string GetCacheKey(int userId) => "UserValuationCashItem_" + userId;

    public static UserValuationCacheItem GetItem(int userId)
    {
        var cacheItem = Cache.Get<UserValuationCacheItem>(GetCacheKey(userId));
        if (cacheItem == null)
        {
           return CreateItemFromDatabase(userId);
        }

        if (!cacheItem.IsBeingRefreshed) return cacheItem;
        
        while (true)
        {
            if (!cacheItem.IsBeingRefreshed)
                return cacheItem;

            Thread.Sleep(10);
        }

    }

    private static UserValuationCacheItem CreateItemFromDatabase(int userId)
    {
        var cacheItem = new UserValuationCacheItem { UserId = userId, IsBeingRefreshed = true };
        AddToCache(cacheItem, userId);
        FillItemFromDatabase(cacheItem);
        cacheItem.IsBeingRefreshed = false;

        return cacheItem;
    }

    private static void AddToCache(UserValuationCacheItem cacheItem, int userId, bool setIsBeingRefreshedToFalse = true)
    {
        Cache.Add(GetCacheKey(userId), cacheItem, TimeSpan.FromMinutes(ExpirationSpanInMinutes), slidingExpiration: true);

        if (setIsBeingRefreshedToFalse)
            cacheItem.IsBeingRefreshed = false;
    }

    private static void FillItemFromDatabase(UserValuationCacheItem cacheItem)
    {
        cacheItem.CategoryValuations = Sl.CategoryValuationRepo.GetByUser(cacheItem.UserId, onlyActiveKnowledge: false);
        cacheItem.QuestionValuations = Sl.QuestionValuationRepo.GetByUser(cacheItem.UserId, onlyActiveKnowledge: false);
    }

    public static IList<QuestionValuation> GetQuestionValuations(int userId)
    {
        return GetItem(userId).QuestionValuations;
    }
}
