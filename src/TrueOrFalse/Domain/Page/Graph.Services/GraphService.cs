/// <summary>
/// Works on the entity cache
/// </summary>
public class GraphService
{
    public static IList<PageCacheItem> Ascendants(int pageId) =>
        Ascendants(EntityCache.GetPage(pageId));

    private static IList<PageCacheItem> Ascendants(PageCacheItem page)
    {
        var allParents = new List<PageCacheItem>();
        var visitedIds = new HashSet<int>();

        var parentIds = new Queue<int>(ParentIds(page));

        while (parentIds.Count > 0)
        {
            int currentParentId = parentIds.Dequeue();

            if (visitedIds.Contains(currentParentId))
            {
                continue;
            }

            var parent = EntityCache.GetPage(currentParentId);
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

    public static void IncrementTotalViewsForAllAscendants(int pageId)
    {
        var ascendants = Ascendants(pageId);
        foreach (var ascendant in ascendants)
            ascendant.TotalViews++;
    }

    public static IList<PageCacheItem> VisibleAscendants(
        int childId,
        PermissionCheck permissionCheck)
    {
        var currentGeneration =
            new HashSet<PageCacheItem>(VisibleParents(childId, permissionCheck));
        var ascendants = new HashSet<PageCacheItem>();

        while (currentGeneration.Count > 0)
        {
            var nextGeneration = new HashSet<PageCacheItem>();

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

    public static List<PageCacheItem> VisibleParents(
        int pageId,
        PermissionCheck permissionCheck)
    {
        var allPages = EntityCache.GetAllPagesList();

        return allPages.SelectMany(c => c.ParentRelations
            .Where(cr => cr.ChildId == pageId && permissionCheck.CanViewPage(cr.ParentId))
            .Select(cr => EntityCache.GetPage(cr.ParentId))).ToList();
    }

    public static List<int> ParentIds(PageCacheItem page)
    {
        return page?.ParentRelations?.Select(cr => cr.ParentId).ToList() ?? new List<int>();
    }

    public static List<PageCacheItem> VisibleChildren(
        int pageId,
        PermissionCheck permissionCheck,
        int userId)
    {
        var visibleChildren = new List<PageCacheItem>();

        var parent = EntityCache.GetPage(pageId);
        foreach (var relation in parent.ChildRelations)
        {
            if (EntityCache.Pages.TryGetValue(relation.ChildId, out var childPage) &&
                permissionCheck.CanView(userId, childPage))
                visibleChildren.Add(childPage);
        }

        return visibleChildren;
    }

    public static IList<PageCacheItem> VisibleDescendants(
        int pageId,
        PermissionCheck permissionCheck,
        int userId)
    {
        var allDescendants = new HashSet<PageCacheItem>();
        var visitedPages = new HashSet<int>();

        void AddDescendants(int id)
        {
            if (visitedPages.Contains(id))
            {
                return;
            }

            visitedPages.Add(id);

            var children = VisibleChildren(id, permissionCheck, userId);
            foreach (var child in children)
            {
                allDescendants.Add(child);
                AddDescendants(child.Id);
            }
        }

        AddDescendants(pageId);

        return allDescendants.ToList();
    }

    public static List<PageCacheItem> Children(PageCacheItem page)
    {
        var childrenIds = page.ChildRelations.Select(r => r.ChildId);
        var children = childrenIds
            .Select(id =>
                EntityCache.Pages.TryGetValue(id, out var childPage)
                    ? childPage
                    : null)
            .Where(c => c != null)
            .ToList();
        return children;
    }

    public static List<PageCacheItem> Children(int pageId) =>
        Children(EntityCache.GetPage(pageId));

    public static IList<PageCacheItem> Descendants(int parentId)
    {
        var descendants = new List<PageCacheItem>();
        var toProcess = new Queue<int>();

        if (EntityCache.Pages.TryGetValue(parentId, out var parentPage))
        {
            foreach (var childRelation in parentPage.ChildRelations)
            {
                toProcess.Enqueue(childRelation.ChildId);
            }
        }

        while (toProcess.Count > 0)
        {
            var currentId = toProcess.Dequeue();
            if (EntityCache.Pages.TryGetValue(currentId, out var currentPage))
            {
                descendants.Add(currentPage);

                foreach (var childRelation in currentPage.ChildRelations)
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