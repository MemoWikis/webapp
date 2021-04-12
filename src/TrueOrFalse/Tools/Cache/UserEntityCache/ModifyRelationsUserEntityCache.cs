using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;

class ModifyRelationsUserEntityCache
{
    public static void CreateRelationsIncludetContentOf(List<CategoryCacheItem> parents, CategoryCacheItem child)
    {
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
        var allCaches = UserEntityCache.GetAllCaches()
            .Select(cache => cache.Value);

        foreach (var cache in allCaches)
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

}

