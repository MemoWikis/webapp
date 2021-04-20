using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

public class ModifyRelationsUserEntityCache
{
    public static void CreateRelationsIncludetContentOf(CategoryCacheItem child)
    {

        var parents = GraphService.GetAllParents(child); 
        foreach (var parent in parents)
        {
            parent.CategoryRelations.Add(new CategoryCacheRelation
            {
                RelatedCategoryId = child.Id,
                CategoryRelationType = CategoryRelationType.IncludesContentOf,
                CategoryId = parent.Id
            });
        }
    }
     
    public static void UpdateRelationsIncludetContentOf(CategoryCacheItem child)
    {
        foreach (var cache in GetAllCaches())
        {
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

                var parentsInner = GraphService.GetDirektParents(result);
                var parentHasBeenAdded = new Dictionary<int, int>();

                while (parentsInner.Count > 0)
                {
                    if (!parentHasBeenAdded.ContainsKey(parentsInner[0]))
                    {
                        parentHasBeenAdded.Add(parentsInner[0], parentsInner[0]);

                        cache.TryGetValue(parentsInner[0], out var directParent);
                        var directParentParents = GraphService.GetDirektParents(directParent);
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

    public static void DeleteIncludetContentOfRelations(CategoryCacheItem category)
    {
        foreach (var cache in GetAllCaches())
        {
            if (cache.ContainsKey(category.Id))
            {
                var allParents = GraphService.GetAllParents(category);
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

    private static IEnumerable<ConcurrentDictionary<int,CategoryCacheItem>> GetAllCaches()
    {
        return UserEntityCache.GetAllCaches()
            .Select(cache => cache.Value);
    }

    public static void DeleteRelationsInChildren(CategoryCacheItem categoryCacheItem)
    {
        DeleteInEntityCache(categoryCacheItem);
        DeleteInUserEntityCache(categoryCacheItem.Id);
     
    }

    private static void DeleteInEntityCache(CategoryCacheItem categoryCacheItem)
    {
        var children = EntityCache.GetChildren(categoryCacheItem);
        foreach (var category in children)
        {
            category.CategoryRelations = category.CategoryRelations.Where(cr => cr.RelatedCategoryId != categoryCacheItem.Id && cr.CategoryId != categoryCacheItem.Id).ToList();
            if (category.CategoryRelations.Count == 0 || !HasChildrenOfRelation(category.CategoryRelations))
                category.CategoryRelations.Add(new CategoryCacheRelation
                {
                    CategoryId = category.Id,
                    CategoryRelationType = CategoryRelationType.IsChildCategoryOf,
                    RelatedCategoryId = RootCategory.RootCategoryId
                });
        }
    }

    private static bool HasChildrenOfRelation( IList<CategoryCacheRelation> relations)
    {
        return relations.Count(r => r.CategoryRelationType == CategoryRelationType.IsChildCategoryOf) != 0; 
    }

    private static void DeleteInUserEntityCache(int categoryId)
    {
        foreach (var userEntityCache in UserEntityCache.GetAllCaches().Values)
        {
            if (userEntityCache.ContainsKey(categoryId))
            {
                var allCacheItems = userEntityCache.Values;
                foreach (var cacheItem in allCacheItems)
                {
                    foreach (var relation in cacheItem.CategoryRelations)
                    {
                        if(relation.RelatedCategoryId == categoryId)
                            cacheItem.CategoryRelations.Remove(relation);

                        if(cacheItem.CategoryRelations.Count== 0 || !HasChildrenOfRelation(cacheItem.CategoryRelations))
                            cacheItem.CategoryRelations.Add(new CategoryCacheRelation
                            {
                                CategoryId = cacheItem.Id,
                                CategoryRelationType = CategoryRelationType.IsChildCategoryOf,
                                RelatedCategoryId = RootCategory.RootCategoryId
                            });
                    }
                }
            }
        }
    }
}

