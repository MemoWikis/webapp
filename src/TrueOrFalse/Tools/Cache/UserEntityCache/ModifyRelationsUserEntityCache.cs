using System.Collections.Generic;
using System.Linq;

public class ModifyRelationsUserEntityCache
{
    public static void AddToParents(CategoryCacheItem child)
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

            if(!parent.HasRelation(newRelation))
                parent.CategoryRelations.Add(newRelation);
        }
    }

    public static void UpdateParents(CategoryCacheItem child)
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
                    parentToRelationAdd?.CategoryRelations.Add(
                        new CategoryCacheRelation
                        {
                            CategoryRelationType = CategoryRelationType.IncludesContentOf, 
                            RelatedCategoryId = child.Id, 
                            CategoryId = parent.Key
                        });
                }  
            }
        }
    }

    public static void DeleteFromAllParents(CategoryCacheItem category)
    {
        foreach (var userEntityCache in UserEntityCache.GetAllCaches().Values)
        {
            if (userEntityCache.ContainsKey(category.Id))
            {
                var allCacheItems = userEntityCache.Values;
                foreach (var cacheItem in allCacheItems)
                {
                    for (var i = 0; i < cacheItem.CategoryRelations.Count; i++)
                    {
                        var relation = cacheItem.CategoryRelations[i];

                        if (relation.RelatedCategoryId == category.Id)
                        {
                            cacheItem.CategoryRelations.Remove(relation);
                            break;
                        }
                            
                    }
                }
            }
        }
    }
}

