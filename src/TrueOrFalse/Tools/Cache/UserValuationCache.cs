using System;
using System.Collections.Generic;
using System.Web;
using Seedworks.Web.State;


public class UserValuationCache
{
    public const int ExpirationSpanInMinutes = 60;

    private static string GetCacheKey(int userId) => "UserValuationCashItem_" + userId;

    public static UserValuationCacheItem GetItem(int userId)
    {
        var cacheItem = Cache.Get<UserValuationCacheItem>(GetCacheKey(userId));
        if (cacheItem == null)
        {
            cacheItem = GetItemFromDatabase(userId);
            UpdateCacheItem(cacheItem, userId);
        }

        return cacheItem;
    }

    private static UserValuationCacheItem GetItemFromDatabase(int userId)
    {
        return new UserValuationCacheItem
        {
            UserId = userId,
            CategoryValuations = Sl.CategoryValuationRepo.GetByUser(userId)
        };
    }

    public static void UpdateValuations(IList<CategoryValuation> categoryValuations, int userId)
    {
        var cacheItem = Cache.Get<UserValuationCacheItem>(GetCacheKey(userId));
        cacheItem.CategoryValuations = categoryValuations;
        UpdateCacheItem(cacheItem, userId);
    }

    public static void UpdateCacheItem(UserValuationCacheItem cacheItem, int userId)
    {
        Cache.Add(GetCacheKey(userId), cacheItem, new TimeSpan(0, ExpirationSpanInMinutes, 0));
    }
}
