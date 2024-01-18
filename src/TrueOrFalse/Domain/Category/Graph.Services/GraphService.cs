
/// <summary>
/// Works on the entity cache
/// </summary>
public class GraphService
{
    public static IList<CategoryCacheItem> Ascendants(int categoryId) =>
        Ascendants(EntityCache.GetCategory(categoryId));

    private static IList<CategoryCacheItem> Ascendants(CategoryCacheItem category)
    {
        var allParents = new List<CategoryCacheItem>();
        var visitedIds = new HashSet<int>();

        var parentIds = new Queue<int>(ParentIds(category));

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

                foreach (var parentId in ParentIds(parent))
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

    public static IList<CategoryCacheItem> Ascendants(int childId, PermissionCheck permissionCheck, bool visibleOnly = false)
    {
        var currentGeneration = Parents(childId, permissionCheck, visibleOnly);
        var nextGeneration = new List<CategoryCacheItem>();
        var ascendants = new List<CategoryCacheItem>();

        while (currentGeneration.Count > 0)
        {
            ascendants.AddRange(currentGeneration);

            foreach (var parent in currentGeneration)
            {
                var parents = Parents(parent.Id, permissionCheck, visibleOnly);
                if (parents.Count > 0)
                {
                    nextGeneration.AddRange(parents);
                }
            }

            currentGeneration = nextGeneration.Except(ascendants).Where(c => c.Id != childId).Distinct().ToList();
            nextGeneration = new List<CategoryCacheItem>();
        }

        ascendants = ascendants.Distinct().ToList();
        var self = ascendants.Find(cci => cci.Id == childId);
        if (self != null)
            ascendants.Remove(self);

        return ascendants;
    }

    public static List<CategoryCacheItem> Parents(int categoryId, PermissionCheck permissionCheck, bool visibleOnly = false)
    {
        var allCategories = EntityCache.GetAllCategories();
        if (visibleOnly)
        {
            return allCategories.SelectMany(c =>
                c.CategoryRelations.Where(cr => cr.ChildCategoryId == categoryId &&
                                                permissionCheck.CanViewCategory(cr.ParentCategoryId))
                    .Select(cr => EntityCache.GetCategory(cr.ParentCategoryId))).ToList();
        }
        return allCategories.SelectMany(c =>
            c.CategoryRelations.Where(cr => cr.ChildCategoryId == categoryId)
                .Select(cr => EntityCache.GetCategory(cr.ParentCategoryId))).ToList();
    }


    public static List<int> ParentIds(CategoryCacheItem category)
    {
        return category?.CategoryRelations?.Select(cr => cr.ParentCategoryId).ToList() ?? new List<int>();
    }
}