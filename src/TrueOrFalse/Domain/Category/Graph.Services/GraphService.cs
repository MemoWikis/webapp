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

    public static void IncrementTotalViewsForAllAscendants(int topicId)
    {
        var ascendants = Ascendants(topicId);
        foreach (var ascendant in ascendants)
            ascendant.TotalViews++;
    }

    public static IList<CategoryCacheItem> VisibleAscendants(
        int childId,
        PermissionCheck permissionCheck)
    {
        var currentGeneration =
            new HashSet<CategoryCacheItem>(VisibleParents(childId, permissionCheck));
        var ascendants = new HashSet<CategoryCacheItem>();

        while (currentGeneration.Count > 0)
        {
            var nextGeneration = new HashSet<CategoryCacheItem>();

            foreach (var parent in currentGeneration)
            {
                if (parent.Id != childId)
                {
                    ascendants.Add(parent);
                }

                foreach (var grandparent in VisibleParents(parent.Id, permissionCheck))
                {
                    if (grandparent.Id != childId)
                    {
                        nextGeneration.Add(grandparent);
                    }
                }
            }

            currentGeneration = nextGeneration;
        }

        return ascendants.ToList();
    }

    public static List<CategoryCacheItem> VisibleParents(
        int categoryId,
        PermissionCheck permissionCheck)
    {
        var allCategories = EntityCache.GetAllCategoriesList();

        return allCategories.SelectMany(c => c.ParentRelations
            .Where(cr => cr.ChildId == categoryId && permissionCheck.CanViewCategory(cr.ParentId))
            .Select(cr => EntityCache.GetCategory(cr.ParentId))).ToList();
    }

    public static List<int> ParentIds(CategoryCacheItem category)
    {
        return category?.ParentRelations?.Select(cr => cr.ParentId).ToList() ?? new List<int>();
    }

    public static List<CategoryCacheItem> VisibleChildren(
        int categoryId,
        PermissionCheck permissionCheck,
        int userId)
    {
        var visibleChildren = new List<CategoryCacheItem>();

        var parent = EntityCache.GetCategory(categoryId);
        foreach (var relation in parent.ChildRelations)
        {
            if (EntityCache.Categories.TryGetValue(relation.ChildId, out var childCategory) &&
                permissionCheck.CanView(userId, childCategory))
                visibleChildren.Add(childCategory);
        }

        return visibleChildren;
    }

    public static IList<CategoryCacheItem> VisibleDescendants(
        int categoryId,
        PermissionCheck permissionCheck,
        int userId)
    {
        var allDescendants = new HashSet<CategoryCacheItem>();
        var visitedCategories = new HashSet<int>();

        void AddDescendants(int id)
        {
            if (visitedCategories.Contains(id))
            {
                return;
            }

            visitedCategories.Add(id);

            var children = VisibleChildren(id, permissionCheck, userId);
            foreach (var child in children)
            {
                allDescendants.Add(child);
                AddDescendants(child.Id);
            }
        }

        AddDescendants(categoryId);

        return allDescendants.ToList();
    }

    public static IList<int> VisibleDescendantIds(
        int categoryId,
        PermissionCheck permissionCheck,
        int userId)
    {
        var allDescendantIds = new HashSet<int>();

        void AddDescendants(int id)
        {
            if (allDescendantIds.Contains(id))
                return;

            allDescendantIds.Add(id);

            var children = VisibleChildren(id, permissionCheck, userId);
            foreach (var child in children)
                AddDescendants(child.Id);
        }

        AddDescendants(categoryId);

        return allDescendantIds.ToList();
    }

    public static List<CategoryCacheItem> Children(CategoryCacheItem category)
    {
        var childrenIds = category.ChildRelations.Select(r => r.ChildId);
        var children = childrenIds
            .Select(id =>
                EntityCache.Categories.TryGetValue(id, out var childCategory)
                    ? childCategory
                    : null)
            .Where(c => c != null)
            .ToList();
        return children;
    }

    public static List<CategoryCacheItem> Children(int categoryId) =>
        Children(EntityCache.GetCategory(categoryId));

    public static IList<CategoryCacheItem> Descendants(int parentId)
    {
        var descendants = new List<CategoryCacheItem>();
        var toProcess = new Queue<int>();

        if (EntityCache.Categories.TryGetValue(parentId, out var parentCategory))
        {
            foreach (var childRelation in parentCategory.ChildRelations)
            {
                toProcess.Enqueue(childRelation.ChildId);
            }
        }

        while (toProcess.Count > 0)
        {
            var currentId = toProcess.Dequeue();
            if (EntityCache.Categories.TryGetValue(currentId, out var currentCategory))
            {
                descendants.Add(currentCategory);

                foreach (var childRelation in currentCategory.ChildRelations)
                {
                    if (!descendants.Any(d => d.Id == childRelation.ChildId) &&
                        !toProcess.Contains(childRelation.ChildId))
                    {
                        toProcess.Enqueue(childRelation.ChildId);
                    }
                }
            }
        }

        return descendants;
    }
}