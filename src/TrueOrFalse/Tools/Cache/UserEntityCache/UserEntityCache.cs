using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;


public class UserEntityCache : BaseCache
{
    /// <summary>
    /// Dictionary[UserId, Dictionary[CategoryId, Category]]
    /// </summary>
    private static readonly ConcurrentDictionary<int, ConcurrentDictionary<int, CategoryCacheItem>> _Categories = 
        new ConcurrentDictionary<int, ConcurrentDictionary<int, CategoryCacheItem>>();

    public static void Init(int userId = -1)
    {
        //Logg.r().Warning("Cache UserEntityCache Init start");

        var user = userId == -1 ?  
            Sl.SessionUser.User : 
            UserCache.GetItem(userId).User;

        //Logg.r().Warning("Cache after user" + " / userId =" + user.Id);

        _Categories[user.Id] = new ConcurrentDictionary<int, CategoryCacheItem>(GraphService
            .GetAllPersonalCategoriesWithRelations(RootCategory.RootCategoryId, userId, true).ToConcurrentDictionary());

        //Logg.r().Warning("Cache after GetAllPersonalCategoriesWithRelations");

        foreach (var cacheItem in _Categories[user.Id])
        {
            ModifyRelationsUserEntityCache.AddToParents(cacheItem.Value);
        }
        Logg.r().Warning("Cache UserEntityCache Init end");
    }

    public static bool IsCacheAvailable(int userId) => _Categories.ContainsKey(userId);

    public static void Add(CategoryCacheItem item, int userId)
    {
        var newItem = item.DeepClone(); 

        var obj = _Categories[userId];
    
        var childRelations = newItem
            .CategoryRelations
            .Where(r => r.CategoryRelationType == CategoryRelationType.IsChildOf);

        var childRelationsInWuWi =
            childRelations.Where(cr => _Categories[Sl.CurrentUserId].ContainsKey(cr.RelatedCategoryId)).ToList(); 
        
        var childRelationsNotInWuwi = childRelations.Where(cr => !_Categories[Sl.CurrentUserId].ContainsKey(cr.RelatedCategoryId));
        foreach (var relation in childRelationsNotInWuwi)
        {
            childRelationsInWuWi.Add(
                new CategoryCacheRelation
                {
                    CategoryId = newItem.Id,
                    CategoryRelationType = CategoryRelationType.IsChildOf,
                    RelatedCategoryId = GetNextParentInWishknowledge(relation.RelatedCategoryId).Id
                }); 
        }

        newItem.CategoryRelations = new List<CategoryCacheRelation>();

        foreach (var categoryRelation in childRelationsInWuWi)
        {
            newItem.CategoryRelations.Add(categoryRelation);
        }
        obj.TryAdd(newItem.Id, newItem );
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

    public static bool IsInWishknowledge(int userId, int categoryId)
    {
        var userCache = GetUserCache(userId);

        if (userCache == null)
            return false;

        return userCache.ContainsKey(categoryId); 
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
    public static void ReInitAllActiveCategoryCaches()
    {
        foreach (var userId in _Categories.Keys)
            Init(userId);
    }
    public static void ChangeCategoryInUserEntityCaches(CategoryCacheItem entityCacheItem, bool isModifyRelations = false)
    {
        var listParents = new ConcurrentDictionary<int, int>();
        foreach (var cacheWithUser in GetAllCaches())
        {
            var cache = cacheWithUser.Value;
            if (cache.ContainsKey(entityCacheItem.Id))
            {
                cache.TryGetValue(entityCacheItem.Id, out var userCacheItem);

                userCacheItem.CategoryRelations = new List<CategoryCacheRelation>();

                foreach (var categoryCacheRelation in userCacheItem.CategoryRelations)
                {
                    CategoryCacheRelation newRelation; 
                    if (cache.ContainsKey(categoryCacheRelation.RelatedCategoryId))
                    {
                       newRelation = new CategoryCacheRelation
                        {
                            CategoryId = entityCacheItem.Id,
                            CategoryRelationType = CategoryRelationType.IsChildOf,
                            RelatedCategoryId = categoryCacheRelation.RelatedCategoryId
                        }; 
                    }
                    else
                    {
                        var nextParentInUserCache = GetNextParentInWishknowledge(entityCacheItem.Id);
                       newRelation = new CategoryCacheRelation
                        {
                            CategoryId = entityCacheItem.Id,
                            CategoryRelationType = CategoryRelationType.IsChildOf,
                            RelatedCategoryId = nextParentInUserCache.Id
                        };
                    }


                    if (!listParents.ContainsKey(newRelation.RelatedCategoryId))
                    {
                        userCacheItem.CategoryRelations.Add(newRelation);
                    }
                }
            }
        }
    }
    public static CategoryCacheItem GetNextParentInWishknowledge(int categoryId)
    {
        var nextParents = EntityCache.GetCategoryCacheItem(categoryId, true).ParentCategories().Distinct().ToList();
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

    public static ConcurrentDictionary<int, ConcurrentDictionary<int, CategoryCacheItem>> GetAllCaches()
    {
        return _Categories; 
    }

    public static bool HasUserCache(int userId) => GetUserCache(userId) != null;  
    public static ConcurrentDictionary<int, CategoryCacheItem> GetUserCache(int userId)
    {
        _Categories.TryGetValue(userId, out var result);
         return result; 
    }

    public static void Clear()
    {
        _Categories.Clear();
    }

    public static List<CategoryCacheItem> GetChildren(int categoryId, int userId)
    {
        var category = GetCategoryWhenNotAvalaibleThenGetNextParent(categoryId,userId);

        var allCategories = GetCategories(userId).Values.ToList();

        return allCategories.SelectMany(c =>
            c.CategoryRelations.Where(cr => 
                    cr.CategoryRelationType == CategoryRelationType.IsChildOf
                    && cr.RelatedCategoryId == category.Id)
                .Select(cr => GetCategory( userId,cr.CategoryId)))
            .ToList();
    }

    public static IEnumerable<CategoryCacheItem> GetByName(int userId, string name)
    {
       return  GetAllCategories(userId).Where(cCI => cCI.Name == name); 
    }

    public static bool IsCategoryCacheKeyAvailable(int userId = -1)
    {
        userId = userId == -1 ? Sl.SessionUser.UserId : userId;
        return _Categories.ContainsKey(userId); 
    }

    public static IEnumerable<int> GetParentsIds(int userId, int topicId)
    {
        _Categories[userId].TryGetValue(topicId, out var topic);
        return topic.CategoryRelations.Where(cr => cr.CategoryRelationType == CategoryRelationType.IsChildOf).Select(cr => cr.RelatedCategoryId);
    }
}


