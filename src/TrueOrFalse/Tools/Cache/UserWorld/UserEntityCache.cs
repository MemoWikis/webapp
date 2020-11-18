using System;
using Seedworks.Web.State;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Tools.Cache.UserWorld;

public class UserEntityCache : BaseCache
{
    private static int _rootCategoryId = 1726; //RootKategorie
    public const int ExpirationSpanInMinutes = 600;

    private static string CategoriesCacheKey(int userId) => "Categories_" + userId;
    private static List<string> CategoriesCacheKeyList = new List<string>();

    public static void Init(bool isTest = false)
    {
        _rootCategoryId = isTest ? 1 : _rootCategoryId;
       var user = Sl.SessionUser.User;

        var cacheItem = new UserEntityCacheItem()
        {
            User = user,
            Categories = new ConcurrentDictionary<int, Category>(GraphService
                .GetAllPersonelCategoriesWithRealtions(_rootCategoryId).ToConcurrentDictionary())
        };
        var categoriesCacheKey = CategoriesCacheKey(user.Id); 
        Cache.Add(categoriesCacheKey, cacheItem.Categories, TimeSpan.FromMinutes(ExpirationSpanInMinutes),true);
        CategoriesCacheKeyList.Add(categoriesCacheKey);
    }

    public static Category GetCategory(int categoryId, int userId)
    {
        if (!CategoriesCacheKeyList.Contains(CategoriesCacheKey(userId)))
            Init();

        return Cache.Get<ConcurrentDictionary<int, Category>>(CategoriesCacheKey(userId))[categoryId]; ;
    }

    public static ConcurrentDictionary<int, Category> GetCategories(int userId)
    {

        return Cache.Get<ConcurrentDictionary<int, Category>>(CategoriesCacheKey(userId)) ?? new ConcurrentDictionary<int, Category>();
    }

    public static void DeleteCacheForUser(int userId)
    {
        Cache.Remove(CategoriesCacheKey(userId));
    }

    public static List<Category> GetChildren(int categoryId, int userId)
    {
        var category = GetCategory(categoryId,userId);

        var allCategories = UserEntityCache.GetCategories(userId).Values.ToList();

        return allCategories.SelectMany(c =>
            c.CategoryRelations.Where(cr => cr.CategoryRelationType == CategoryRelationType.IsChildCategoryOf && cr.RelatedCategory.Id == category.Id)
                .Select(cr => cr.Category)).ToList();
    }

}


