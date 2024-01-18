
/// <summary>
/// Works on the entity cache
/// </summary>
public class GraphService
{
    public static IList<CategoryCacheItem> GetAllParents(int categoryId) =>
        GetAllParents(EntityCache.GetCategory(categoryId));

    private static IList<CategoryCacheItem> GetAllParents(CategoryCacheItem category)
    {
        var allParents = new List<CategoryCacheItem>();
        var visitedIds = new HashSet<int>();

        var parentIds = new Queue<int>(GetDirectParentIds(category));

        while (parentIds.Count > 0)
        {
            int currentParentId = parentIds.Dequeue();

            if (visitedIds.Contains(currentParentId))
            {
                continue;
            }

            var parent = EntityCache.GetCategory(currentParentId);
            if (parent != null)
            {
                allParents.Add(parent);
                visitedIds.Add(currentParentId);

                foreach (var parentId in GetDirectParentIds(parent))
                {
                    if (!visitedIds.Contains(parentId))
                    {
                        parentIds.Enqueue(parentId);
                    }
                }
            }
        }

        return allParents;
    }

    public static List<int> GetDirectParentIds(CategoryCacheItem category)
    {
        return category?.CategoryRelations?.Select(cr => cr.ParentCategoryId).ToList() ?? new List<int>();
    }
}