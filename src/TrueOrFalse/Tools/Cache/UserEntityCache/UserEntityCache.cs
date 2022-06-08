using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Tools.Cache.UserWorld;


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
            SessionUser.User : 
            UserCache.GetItem(userId).User;

        _Categories[user.Id] = new ConcurrentDictionary<int, CategoryCacheItem>(GraphService
            .GetAllWuwiCategoriesWithRelations(RootCategory.RootCategoryId, userId, true).ToConcurrentDictionary());

        foreach (var cacheItem in _Categories[user.Id])
        {
            ModifyRelationsUserEntityCache.AddToParents(cacheItem.Value);
        }

    }

    public static bool IsCacheAvailable(int userId) => _Categories.ContainsKey(userId);

    public static void Add(CategoryCacheItem item, int userId)
    {
        var newItem = item.DeepClone(); 

        var obj = _Categories[userId];

        var childRelations = newItem
            .CategoryRelations;

        var childRelationsInWuWi =
            childRelations.Where(cr => _Categories[Sl.CurrentUserId].ContainsKey(cr.RelatedCategoryId)).ToList(); 
        
        var childRelationsNotInWuwi = childRelations.Where(cr => !_Categories[Sl.CurrentUserId].ContainsKey(cr.RelatedCategoryId));
        foreach (var relation in childRelationsNotInWuwi)
        {
            childRelationsInWuWi.Add(
                new CategoryCacheRelation
                {
                    CategoryId = newItem.Id,
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
        var allCategories = GetAllCategoriesAsDictionary(userId);
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
    public static ConcurrentDictionary<int, CategoryCacheItem> GetAllCategoriesAsDictionary(int userId)
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
        if (SessionUser.UserId > 0) 
            _Categories.TryRemove(SessionUser.UserId, out _);
    }


    public static void DeleteCacheForUser(int userId)
    {
        _Categories.TryRemove(userId, out _);
    }
    public static void DeleteCategory(int categoryId)
    {
        foreach (var userCache in _Categories.Values)
        {
            if (userCache.ContainsKey(categoryId))
                userCache.TryRemove(categoryId, out var outobj); 
        }
    }
    public static void ReInitAllActiveCategoryCaches()
    {
        foreach (var userId in _Categories.Keys)
            Init(userId);
    }
    public static void ChangeCategoryInUserEntityCaches(CategoryCacheItem entityCacheItem)
    {
        var listParents = new ConcurrentDictionary<int, int>();
        foreach (var cacheWithUser in GetAllCaches())
        {
            var cache = cacheWithUser.Value;
            if (cache.ContainsKey(entityCacheItem.Id))
            {
                cache.TryGetValue(entityCacheItem.Id, out var userCacheItem);

                userCacheItem.CategoryRelations = new List<CategoryCacheRelation>();
                userCacheItem.Name = entityCacheItem.Name;
                userCacheItem.Content = entityCacheItem.Content;
                userCacheItem.Visibility = entityCacheItem.Visibility; 

                foreach (var categoryCacheRelation in userCacheItem.CategoryRelations)
                {
                    CategoryCacheRelation newRelation; 
                    if (cache.ContainsKey(categoryCacheRelation.RelatedCategoryId))
                    {
                       newRelation = new CategoryCacheRelation
                        {
                            CategoryId = entityCacheItem.Id,
                            RelatedCategoryId = categoryCacheRelation.RelatedCategoryId
                        }; 
                    }
                    else
                    {
                        var nextParentInUserCache = GetNextParentInWishknowledge(entityCacheItem.Id);
                       newRelation = new CategoryCacheRelation
                        {
                            CategoryId = entityCacheItem.Id,
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
        var nextParents = EntityCache.GetCategory(categoryId, true).ParentCategories().Distinct().ToList();
        var user = SessionUser.User; 
            
        while (nextParents.Count > 0)
        {
            var nextParent = nextParents.First();

            if (nextParent.IsInWishknowledge())
            {
                return nextParent;
            }

            if (nextParents.Count == 1 && user.IsStartTopicTopicId(nextParent.Id))
                return nextParent;

            var parentHelperList = nextParent.ParentCategories();
            nextParents.RemoveAt(0);

            foreach (var parent in parentHelperList)
            {
                nextParents.Add(parent);
            }

            nextParents.Distinct();
        }

        if (nextParents.Count == 0)
        {
            return EntityCache.GetCategory(user.StartTopicId, getDataFromEntityCache: true);
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

        var allCategories = GetAllCategoriesAsDictionary(userId).Values.ToList();

        return allCategories.SelectMany(c =>
            c.CategoryRelations.Where(cr => cr.RelatedCategoryId == category.Id)
                .Select(cr => GetCategory( userId,cr.CategoryId)))
            .ToList();
    }

    public static IEnumerable<CategoryCacheItem> GetByName(int userId, string name)
    {
       return  GetAllCategories(userId).Where(cCI => cCI.Name == name); 
    }

    public static bool IsCategoryCacheKeyAvailable(int userId = -1)
    {
        userId = userId == -1 ? SessionUser.UserId : userId;
        return _Categories.ContainsKey(userId); 
    }

    public static IEnumerable<int> GetParentsIds(int userId, int topicId)
    {
        _Categories[userId].TryGetValue(topicId, out var topic);
        return topic.CategoryRelations.Select(cr => cr.RelatedCategoryId);
    }
}


