using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Seedworks.Lib;
using TrueOrFalse.Tools;

public class UserEntityCache : BaseCache
{
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<int, Category>> _Categories = new ConcurrentDictionary<string, ConcurrentDictionary<int, Category>>(); 

    public static string CategoriesCacheKey(int userId) => "Categories_" + userId;
    private static readonly List<string> CategoriesCacheKeyList = new List<string>();

    public static void Init(int userId = -1)
    {
        var user = userId == -1 ?  Sl.SessionUser.User : UserCache.GetItem(userId).User;

        var categories = new ConcurrentDictionary<int, Category>(GraphService
            .GetAllPersonalCategoriesWithRelations(RootCategory.RootCategoryId, userId, true).ToConcurrentDictionary()); 
    
        var categoriesCacheKey = CategoriesCacheKey(user.Id);
        
        _Categories[categoriesCacheKey] = categories; 

        if(!CategoriesCacheKeyList.Contains(categoriesCacheKey))
            CategoriesCacheKeyList.Add(categoriesCacheKey);
    }

    public static ConcurrentDictionary<int, Category> GetCategories(string categoryCacheKey)
    {
        if (_Categories.ContainsKey(categoryCacheKey))
            return _Categories[categoryCacheKey];

        return new ConcurrentDictionary<int, Category>();
    }

    public static Category GetCategory(int categoryId, int userId)
    {
        if (!CategoriesCacheKeyList.Contains(CategoriesCacheKey(userId)))
            Init();
 
        if (_Categories[CategoriesCacheKey(userId)].ContainsKey(categoryId))
            return _Categories[CategoriesCacheKey(userId)][categoryId];

        return _Categories[CategoriesCacheKey(userId)][GetNextParentInWishknowledge(categoryId).Id];
    }

    public static ConcurrentDictionary<int, Category> GetCategories(int userId)
    {
        if (!CategoriesCacheKeyList.Contains(CategoriesCacheKey(userId)))
            Init();

        ConcurrentDictionary<int, Category> result; 

        _Categories.TryGetValue(CategoriesCacheKey(userId), out result);

        if(result == null)
            Logg.r().Error("CategoryCache is null, UserId=" + userId );

        return result;
    }

    public static void DeleteCacheForUser()
    {
        if (Sl.SessionUser != null)
        {
            var cacheKey = CategoriesCacheKey(Sl.SessionUser.UserId);
            _Categories.TryRemove(cacheKey, out _);
            CategoriesCacheKeyList.Remove(cacheKey);
        }
    }

    public static List<Category> GetChildren(int categoryId, int userId)
    {
        var category = GetCategory(categoryId,userId);

        var allCategories = GetCategories(userId).Values.ToList();

        return allCategories.SelectMany(c =>
            c.CategoryRelations.Where(cr => 
                    cr.CategoryRelationType == CategoryRelationType.IsChildCategoryOf
                    && cr.RelatedCategory.Id == category.Id)
                .Select(cr => cr.Category))
            .ToList();
    }

    public static Category GetNextParentInWishknowledge(int categoryId)
    {
        var nextParents = EntityCache.GetCategory(categoryId,true).ParentCategories().Distinct().ToList();
        if (nextParents.Count == 0)
            nextParents.Add(RootCategory.Get);  // Has Category no parent then add Rootcategory

        while (nextParents.Count > 0)
        {
            var nextParent = nextParents.First();

            if (nextParent.IsInWishknowledge())
            {
               return nextParent;
            }

            if (nextParents.Count == 1 && nextParents.First().IsRootCategory)
                return nextParents.First(); 

                var parentHelperList = nextParent.ParentCategories();
                nextParents.RemoveAt(0);

                foreach (var parent in parentHelperList)
                {
                   nextParents.Add(parent); 
                }

                nextParents.Distinct();
        }
        Logg.r().Error("Root category Not available/ UserEntityCache/NextParentInWishknowledge");
        throw new NotImplementedException();
    }

    public static void ChangeAllActiveCategoryCaches()
    {
        foreach (var CategoryCacheKey in CategoriesCacheKeyList)
        {
            var firstChar = CategoryCacheKey.IndexOf("_", StringComparison.Ordinal) + 1 ;

            var length = (CategoryCacheKey.Length - 1) - (firstChar - 1); 
            var userId = CategoryCacheKey.Substring(firstChar, length).ToInt(); 
            
            Init(userId);
        }
    }

    public static void ChangeCategoryInUserEntityCaches(Category category)
    {
        foreach (var UserCategoriesCache in _Categories.Values)
        {
            if (UserCategoriesCache.ContainsKey(category.Id))
                UserCategoriesCache.TryUpdate(category.Id, category, UserCategoriesCache[category.Id]);
        }
    }

    public static bool IsCategoryCacheKeyAvailable(int userId = -1)
    {
        userId = userId == -1 ? Sl.SessionUser.UserId : userId;
        return CategoriesCacheKeyList.Contains(CategoriesCacheKey(userId)); 
    }
}


