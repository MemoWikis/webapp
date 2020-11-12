using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using FluentNHibernate.Utils;
using TrueOrFalse.Tools.Cache.UserWorld;

public class UserEntityCache : BaseCache
{
    private static int _rootCategoryId = 1;

    private static string CategoriesCacheKey(int userId) => "Categories_" + userId;


    public static void Init()
    {
        var user = Sl.SessionUser.User;

        var cacheItem = new UserWorldCacheItem()
        {
            User = user,
            Categories = new ConcurrentDictionary<int, Category>(GraphService
                .GetAllPersonelCategoriesWithRealtions(_rootCategoryId).ToConcurrentDictionary())
        };
        IntoForeverCache(CategoriesCacheKey(user.Id), cacheItem.Categories);

    }

    public static UserWorldCacheItem CreateItemFromDatabase(int userId)
    {
        var user = Sl.UserRepo.GetById(userId);
        var cacheItem = new UserWorldCacheItem()
        {
            User = user,
            Categories = new ConcurrentDictionary<int, Category>(GraphService.GetAllPersonelCategoriesWithRealtions(_rootCategoryId).ToConcurrentDictionary())
        };


        IntoForeverCache(CategoriesCacheKey(user.Id), cacheItem.Categories);

        return cacheItem;
    }

    public static UserWorldCacheItem GetUserWorldItem(int userId)
    {
        var categories = (ConcurrentDictionary<int, Category>)HttpRuntime.Cache[CategoriesCacheKey(userId)];
        var user = Sl.UserRepo.GetById(userId);

        var result = new UserWorldCacheItem
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

        return (ConcurrentDictionary<int, Category>) HttpRuntime.Cache[CategoriesCacheKey(userId)]; 
    }
}


