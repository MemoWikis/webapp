
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

    public static IList<CategoryCacheItem> VisibleAscendants(int childId, PermissionCheck permissionCheck)
    {
        var currentGeneration = new HashSet<CategoryCacheItem>(VisibleParents(childId, permissionCheck));
        var ascendants = new HashSet<CategoryCacheItem>();

        while (currentGeneration.Count > 0)
        {
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
                        currentGeneration.Add(grandparent);
                    }
                }
            }

            currentGeneration = new HashSet<CategoryCacheItem>(currentGeneration.Except(ascendants));
        }

        return ascendants.ToList();
    }


    public static List<CategoryCacheItem> VisibleParents(int categoryId, PermissionCheck permissionCheck)
    {
        var allCategories = EntityCache.GetAllCategoriesList();

        return allCategories.SelectMany(c => c.CategoryRelations
            .Where(cr => cr.ChildCategoryId == categoryId && permissionCheck.CanViewCategory(cr.ParentCategoryId))
                    .Select(cr => EntityCache.GetCategory(cr.ParentCategoryId))).ToList();

    }


    public static List<int> ParentIds(CategoryCacheItem category)
    {
        return category?.CategoryRelations?.Select(cr => cr.ParentCategoryId).ToList() ?? new List<int>();
    }

    public static List<CategoryCacheItem> VisibleChildren(int categoryId, PermissionCheck permissionCheck, int userId)
    {
        var visibleChildren = new List<CategoryCacheItem>();

        foreach (var category in EntityCache.Categories.Values)
        {
            foreach (var relation in category.CategoryRelations)
            {
                if (relation.ParentCategoryId != categoryId)
                    continue;

                if (EntityCache.Categories.TryGetValue(category.Id, out var childCategory) &&
                    permissionCheck.CanView(userId, childCategory))
                    visibleChildren.Add(childCategory);
            }
        }

        return visibleChildren;
    }

    public static IList<CategoryCacheItem> VisibleDescendants(int categoryId, PermissionCheck permissionCheck, int userId)
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

    public static List<CategoryCacheItem> Children(CategoryCacheItem category) => Children(category.Id);
    public static List<CategoryCacheItem> Children(int categoryId)
    {
        var childrenIds = EntityCache.Categories.Values
            .SelectMany(c => c.CategoryRelations)
            .Where(cr => cr.ParentCategoryId == categoryId)
            .Select(cr => cr.ChildCategoryId)
            .Distinct();

        var children = childrenIds
            .Select(id => EntityCache.Categories.TryGetValue(id, out var childCategory) ? childCategory : null)
            .Where(c => c != null)
            .ToList();
        return children;
    }

    public static IList<CategoryCacheItem> Descendants(int parentId)
    {
        var descendants = new List<CategoryCacheItem>();
        var toProcess = new Queue<int>(new[] { parentId });

        while (toProcess.Count > 0)
        {
            var currentId = toProcess.Dequeue();

            if (currentId != parentId && EntityCache.Categories.TryGetValue(currentId, out var currentCategory))
                descendants.Add(currentCategory);

            foreach (var potentialChild in EntityCache.Categories.Values)
            {
                foreach (var relation in potentialChild.CategoryRelations)
                {
                    if (relation.ParentCategoryId == currentId && !descendants.Any(d => d.Id == potentialChild.Id) && !toProcess.Contains(potentialChild.Id))
                        toProcess.Enqueue(potentialChild.Id);
                }
            }
        }
        return descendants;
    }
}