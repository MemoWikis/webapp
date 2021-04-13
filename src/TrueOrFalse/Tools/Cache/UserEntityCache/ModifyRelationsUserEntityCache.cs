using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;

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


}

