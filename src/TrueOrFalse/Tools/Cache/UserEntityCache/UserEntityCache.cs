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
    private static readonly ConcurrentDictionary<int, ConcurrentDictionary<int, Category>> _Categories = 
        new ConcurrentDictionary<int, ConcurrentDictionary<int, Category>>();

    public static void Init(int userId = -1)
    {
        var user = userId == -1 ?  
            Sl.SessionUser.User : 
            UserCache.GetItem(userId).User;

        var categories = new ConcurrentDictionary<int, Category>(GraphService
            .GetAllPersonalCategoriesWithRelations(RootCategory.RootCategoryId, userId, true).ToConcurrentDictionary()); 
        
        _Categories[user.Id] = categories;
    }

    public static void Add(Category category, int userId)
    {
        var obj = _Categories[userId];
        var categoryClone = category.DeepClone();
        var childRelations = categoryClone.CategoryRelations.Where(r => r.CategoryRelationType == CategoryRelationType.IsChildCategoryOf);

        categoryClone.CategoryRelations = new List<CategoryRelation>();

        foreach (var categoryRelation in childRelations)
        {
            categoryClone.CategoryRelations.Add(categoryRelation);
        }
        obj.TryAdd(category.Id, categoryClone );
    }

    public static Category GetCategory(int categoryId, int userId)
    {
        if (!_Categories.ContainsKey(userId))
            Init();
 
        if (_Categories[userId].ContainsKey(categoryId)) 
            return _Categories[userId][categoryId];

        return _Categories[userId][GetNextParentInWishknowledge(categoryId).Id];
    }

    public static ConcurrentDictionary<int, Category> GetCategories(int userId)
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

    public static void ReInitAllActiveCategoryCaches()
    {
        foreach (var userId in _Categories.Keys) 
            Init(userId);
    }

    public static void ChangeCategoryInUserEntityCaches(Category category)
    {
        foreach (var categories in _Categories.Values)
        {
            if (categories.ContainsKey(category.Id))
                categories.TryUpdate(category.Id, category, categories[category.Id]);
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


