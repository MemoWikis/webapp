using System.Collections.Generic;
using System.Linq;

public class ModifyRelationsUserEntityCache
{
    public static void CreateRelationsIncludedContentOf(CategoryCacheItem child)
    {
        var parents = GraphService.GetAllParentsFromUserEntityCache(Sl.CurrentUserId, child); 
        foreach (var parent in parents)
        {
            var newRelation = new CategoryCacheRelation
            {
                RelatedCategoryId = child.Id,
                CategoryRelationType = CategoryRelationType.IncludesContentOf,
                CategoryId = parent.Id
            };

            var hasEqualRelation = false; 
                foreach (var categoryRelation in parent.CategoryRelations)
                {
                    if (IsCategorRelationEqual(categoryRelation, newRelation))
                    {
                        hasEqualRelation = true; 
                        break; 
                    }
                }

                if(!hasEqualRelation)
                    parent.CategoryRelations.Add(newRelation);
        }
    }

    private static bool IsCategorRelationEqual(CategoryCacheRelation relation1, CategoryCacheRelation relation2)
    {
        return relation1.RelatedCategoryId == relation2.RelatedCategoryId &&
               relation1.CategoryRelationType == relation2.CategoryRelationType &&
               relation1.CategoryId == relation2.CategoryId; 
    }

    public static void UpdateIncludedContentOf(CategoryCacheItem child)
    {
        foreach (var cacheWithUser in UserEntityCache.GetAllCaches())
        {
            var cache = cacheWithUser.Value; 

            if (cache.ContainsKey(child.Id))
            {
                cache.TryGetValue(child.Id, out var result);
                var allCategoryCacheItems = cache.Values.ToList();
                foreach (var categoryCacheItem in allCategoryCacheItems)
                {
                    for (var i = 0; i < categoryCacheItem.CategoryRelations.Count; i++)
                    {
                        if (categoryCacheItem.CategoryRelations[i].CategoryRelationType == CategoryRelationType.IncludesContentOf &&
                            categoryCacheItem.CategoryRelations[i].RelatedCategoryId == child.Id)
                            categoryCacheItem.CategoryRelations.Remove(categoryCacheItem.CategoryRelations[i]);
                    }
                }

                var parentsInner = GraphService.GetDirectParents(result);
                var parentHasBeenAdded = new Dictionary<int, int>();

                while (parentsInner.Count > 0)
                {
                    if (!parentHasBeenAdded.ContainsKey(parentsInner[0]))
                    {
                        parentHasBeenAdded.Add(parentsInner[0], parentsInner[0]);

                        cache.TryGetValue(parentsInner[0], out var directParent);
                        var directParentParents = GraphService.GetDirectParents(directParent);
                        foreach (var parentParent in directParentParents)
                        {
                            parentsInner.Add(parentParent);
                        }
                    }
                    parentsInner.RemoveAt(0);
                }

                foreach (var parent in parentHasBeenAdded)
                {
                    cache.TryGetValue(parent.Key, out var parentToRelationAdd);
                    parentToRelationAdd.CategoryRelations.Add(new CategoryCacheRelation { CategoryRelationType = CategoryRelationType.IncludesContentOf, RelatedCategoryId = child.Id, CategoryId = parent.Key });
                }  
            }
        }
    }

    public static void DeleteIncludedContentOf(CategoryCacheItem category)
    {
        foreach (var cacheWithUser in UserEntityCache.GetAllCaches())
        {
            var userId = cacheWithUser.Key;
            var cache = cacheWithUser.Value; 
            if (cache.ContainsKey(category.Id))
            {
                var allParents = GraphService.GetAllParentsFromUserEntityCache(userId, category);
                foreach (var parent in allParents)
                {
                    for (int i = 0; i < parent.CategoryRelations.Count; i++)
                    {
                        if (parent.CategoryRelations[i].RelatedCategoryId == category.Id && parent.CategoryRelations[i].CategoryRelationType == CategoryRelationType.IncludesContentOf)
                            parent.CategoryRelations.RemoveAt(i);
                    }
                }
            }
            cache.TryRemove(category.Id, out var result); 
        }
    }

    public static void Delete(CategoryCacheItem categoryFromEntityCache)
    {
        DeleteIncludedContentOf(categoryFromEntityCache);

        foreach (var userEntityCache in UserEntityCache.GetAllCaches().Values)
        {
            if (userEntityCache.ContainsKey(categoryFromEntityCache.Id))
            {
                var allCacheItems = userEntityCache.Values;
                foreach (var cacheItem in allCacheItems)
                {
                    foreach (var relation in cacheItem.CategoryRelations)
                    {
                        if(relation.RelatedCategoryId == categoryFromEntityCache.Id)
                            cacheItem.CategoryRelations.Remove(relation);
                    }
                }
            }
        }
    }
}

