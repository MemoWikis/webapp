using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

public class GraphService
{
    public static IList<CategoryCacheItem> GetAllParentsFromEntityCache(int categoryId) =>
        GetAllParentsFromEntityCache(EntityCache.GetCategory(categoryId));

    private static IList<CategoryCacheItem> GetAllParentsFromEntityCache(CategoryCacheItem category)
    {
        var parentIds = GetDirectParentIds(category);
        var allParents = new List<CategoryCacheItem>();
        var deletedIds = new Dictionary<int, int>();

        while (parentIds.Count > 0)
        {
            var parent = EntityCache.GetCategory(parentIds[0]);

            if (!deletedIds.ContainsKey(parentIds[0]))
            {
                allParents.Add(parent); //Avoidance of circular references

                deletedIds.Add(parentIds[0], parentIds[0]);
                var currentParents = GetDirectParentIds(parent);
                foreach (var currentParent in currentParents)
                {
                    parentIds.Add(currentParent);
                }
            }

            parentIds.RemoveAt(0);
        }

        return allParents;
    }

    public static List<int> GetDirectParentIds(CategoryCacheItem category)
    {
        if (category == null)
        {
            return new List<int>();
        }

        return category.CategoryRelations
            .Select(cr => cr.RelatedCategoryId).ToList();
    }

    public static ConcurrentDictionary<int, CategoryCacheItem> AddChildrenIdsToCategoryCacheData(
        ConcurrentDictionary<int, CategoryCacheItem> categories)
    {
        foreach (var category in categories.Values)
        {
            foreach (var categoryRelation in category.CategoryRelations)
            {
                if (categories.ContainsKey(categoryRelation.RelatedCategoryId))
                {
                    categories[categoryRelation.RelatedCategoryId].CachedData
                        .AddChildId(categories[categoryRelation.CategoryId].Id);
                }
            }
        }

        return categories;
    }

    public static void AutomaticInclusionOfChildCategoriesForEntityCacheAndDbCreate(CategoryCacheItem category, int userId)
    {
        var parentsFromParentCategories = GetAllParentsFromEntityCache(category.Id);

        foreach (var parent in parentsFromParentCategories)
        {
            parent.UpdateCountQuestionsAggregated(userId);
        }
    }
}