using System;
using Seedworks.Web.State;
using System.Collections.Concurrent;
using TrueOrFalse.Tools.Cache.UserWorld;

public class UserEntityCache : BaseCache
{
    private static int _rootCategoryId = 1;
    public const int ExpirationSpanInMinutes = 600;

    private static string CategoriesCacheKey(int userId) => "Categories_" + userId;


    public static void Init()
    {
        var user = Sl.SessionUser.User;
        var t = GraphService
            .GetAllPersonelCategoriesWithRealtions(_rootCategoryId);
        var cacheItem = new UserEntityCacheItem()
        {
            User = user,
            Categories = new ConcurrentDictionary<int, Category>(GraphService
                .GetAllPersonelCategoriesWithRealtions(_rootCategoryId).ToConcurrentDictionary())
        };
        Cache.Add(CategoriesCacheKey(user.Id), cacheItem.Categories, TimeSpan.FromMinutes(ExpirationSpanInMinutes),true);

    }

    public static UserEntityCacheItem GetUserWorldItem(int userId)
    {
        var categories = Cache.Get<ConcurrentDictionary<int, Category>>(CategoriesCacheKey(userId));
        var user = Sl.UserRepo.GetById(userId);

        var result = new UserEntityCacheItem
        {
            User = user ?? Sl.UserRepo.GetById(userId),
            Categories = categories ?? new ConcurrentDictionary<int, Category>(GraphService
                .GetAllPersonelCategoriesWithRealtions(_rootCategoryId) 
                .ToConcurrentDictionary())
        };
        return result;
    }


    public static ConcurrentDictionary<int, Category> GetCategories(int userId)
    {
        return Cache.Get<ConcurrentDictionary<int, Category>>(CategoriesCacheKey(userId));
    }
}


