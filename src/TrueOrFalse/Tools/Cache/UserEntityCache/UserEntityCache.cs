using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Seedworks.Lib;

public class UserEntityCache : BaseCache
{
    /// <summary>
    /// Dictionary[UserId, Dictionary[CategoryId, Category]]
    /// </summary>
    private static readonly ConcurrentDictionary<int, ConcurrentDictionary<int, CategoryCacheItem>> _Categories = 
        new ConcurrentDictionary<int, ConcurrentDictionary<int, CategoryCacheItem>>();

    public static void Init(int userId = -1)
    {
        var user = userId == -1 ?  
            Sl.SessionUser.User : 
            UserCache.GetItem(userId).User;

        _Categories[user.Id] = new ConcurrentDictionary<int, CategoryCacheItem>(GraphService
            .GetAllPersonalCategoriesWithRelations(RootCategory.RootCategoryId, userId, true).ToConcurrentDictionary());
    }

    public static void Add(CategoryCacheItem item, int userId)
    {
        var obj = _Categories[userId];
    
        var childRelations = item
            .CategoryRelations
            .Where(r => r.CategoryRelationType == CategoryRelationType.IsChildCategoryOf);

        item.CategoryRelations = new List<CategoryCacheRelation>();

        foreach (var categoryRelation in childRelations)
        {
            item.CategoryRelations.Add(categoryRelation);
        }
        obj.TryAdd(item.Id, item );
    }

    public static CategoryCacheItem GetCategoryWhenNotAvalaibleThenGetNextParent(int categoryId, int userId)
    {
        if (!_Categories.ContainsKey(userId))
            Init();
 
        if (_Categories[userId].ContainsKey(categoryId)) 
            return _Categories[userId][categoryId];

        return _Categories[userId][GetNextParentInWishknowledge(categoryId).Id];
    }

    public static CategoryCacheItem GetCategory(int userId, int categoryId)
    {
        var allCategories = GetCategories(userId);
        CategoryCacheItem result;
        return allCategories.TryGetValue(categoryId, out result) ?  result : null;
    }

    public static IEnumerable<CategoryCacheItem> GetAllCategories(int userId) => _Categories[userId].Values;  
    public static ConcurrentDictionary<int, CategoryCacheItem> GetCategories(int userId)
    {
        if (!_Categories.ContainsKey(userId))
            Init();

        _Categories.TryGetValue(userId, out var result);

        if(result == null)
            Logg.r().Error("CategoryCache is null, UserId=" + userId );

        return result;
    }

    public static void DeleteCacheForUser()
    {
        if (Sl.SessionUser != null) 
            _Categories.TryRemove(Sl.SessionUser.UserId, out _);
    }

    public static List<CategoryCacheItem> GetChildren(int categoryId, int userId)
    {
        var category = GetCategoryWhenNotAvalaibleThenGetNextParent(categoryId,userId);

        var allCategories = GetCategories(userId).Values.ToList();

        return allCategories.SelectMany(c =>
            c.CategoryRelations.Where(cr => 
                    cr.CategoryRelationType == CategoryRelationType.IsChildCategoryOf
                    && cr.RelatedCategoryId == category.Id)
                .Select(cr => GetCategory( userId,cr.CategoryId)))
            .ToList();
    }

    public static CategoryCacheItem GetNextParentInWishknowledge(int categoryId)
    {
        var nextParents = EntityCache.GetCategoryCacheItem(categoryId,true).ParentCategories().Distinct().ToList();
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

    public static void ReInitAllActiveCategoryCaches()
    {
        foreach (var userId in _Categories.Keys) 
            Init(userId);
    }

    public static void ChangeCategoryInUserEntityCaches(CategoryCacheItem categoryCacheItem)
    {
        foreach (var categories in _Categories.Values)
        {
            if (categories.ContainsKey(categoryCacheItem.Id))
                categories.TryUpdate(categoryCacheItem.Id, categoryCacheItem, categories[categoryCacheItem.Id]);
        }
    }

    public static bool IsCategoryCacheKeyAvailable(int userId = -1)
    {
        userId = userId == -1 ? Sl.SessionUser.UserId : userId;
        return _Categories.ContainsKey(userId); 
    }

    public static void Clear()
    {
        _Categories.Clear();
    }
}


