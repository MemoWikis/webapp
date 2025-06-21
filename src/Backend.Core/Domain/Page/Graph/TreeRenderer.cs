using System.Text;

public class TreeRenderer
{
    // Constants for ASCII tree drawing
    private const string Vertical = "│   "; // Vertical line and indent (for continuing branches)
    private const string Space = "    "; // Indentation (for branches that have ended)
    private const string Cross = "├── "; // Tee branch for non-last child
    private const string Corner = "└── "; // Corner branch for last child        

    /// <summary>
    /// Generates a recursive ASCII diagram of this page and its children, 
    /// showing the hierarchy (tree structure) and the sibling order at each level.
    /// </summary>
    public static string ToAsciiDiagram(PageCacheItem pageCacheItem)
    {
        var stringBuilder = new StringBuilder();

        // Print the current page's name as root of this diagram (no prefix for top level)
        stringBuilder.AppendLine(pageCacheItem.Name);

        // Recursively print all children, if any, with proper indentation and ordering
        var orderedChildren = GetOrderedChildren(pageCacheItem);
        for (int index = 0; index < orderedChildren.Count; index++)
        {
            bool isLastChild = (index == orderedChildren.Count - 1);
            AppendChildDiagram(stringBuilder, orderedChildren[index], indent: "", isLastChild: isLastChild);
        }

        // After listing all top-level children, append the sibling order for this level (if any children)
        if (orderedChildren.Count > 0)
        {
            string siblingLine = string.Join(" → ", orderedChildren.Select(child => $"[{child.Name}]"));
            stringBuilder.AppendLine(siblingLine);
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Helper method to recursively append the ASCII diagram for a child node.
    /// </summary>
    /// <param name="stringBuilder">The StringBuilder accumulating the output.</param>
    /// <param name="child">The child PageCacheItem to append.</param>
    /// <param name="indent">The prefix string for indentation from the parent levels.</param>
    /// <param name="isLastChild">True if this child is the last in its sibling list.</param>
    private static void AppendChildDiagram(StringBuilder stringBuilder, PageCacheItem child, string indent, bool isLastChild)
    {
        // Append the indent and branch prefix for this child
        stringBuilder.Append(indent);
        stringBuilder.Append(isLastChild ? Corner : Cross);
        stringBuilder.AppendLine(child.Name);

        // Determine indent for this child's children (extend the current indent)
        string childIndent = indent + (isLastChild ? Space : Vertical);

        // Recurse into children of this node, if any
        var orderedChildren = GetOrderedChildren(child);
        for (int index = 0; index < orderedChildren.Count; index++)
        {
            bool isLast = (index == orderedChildren.Count - 1);
            AppendChildDiagram(stringBuilder, orderedChildren[index], childIndent, isLast);
        }

        // After printing all children of this node, append the sibling order line if there are children
        if (orderedChildren.Count > 0)
        {
            string siblingsLine = string.Join(" → ", orderedChildren.Select(c => $"[{c.Name}]"));
            stringBuilder.Append(childIndent);
            stringBuilder.AppendLine(siblingsLine);
        }
    }

    /// <summary>
    /// Retrieves the list of child PageCacheItems in the correct order, based on PreviousId/NextId links.
    /// </summary>
    private static List<PageCacheItem> GetOrderedChildren(PageCacheItem parent)
    {
        var orderedChildren = new List<PageCacheItem>();
        if (parent.ChildRelations == null || parent.ChildRelations.Count == 0)
            return orderedChildren;

        // 1) First child (no PreviousId)
        var currentRelation = parent.ChildRelations
            .FirstOrDefault(r => r.PreviousId == null);

        if (currentRelation == null)
        {
            // Data inconsistency - return unordered children as fallback
            orderedChildren.AddRange(parent.ChildRelations
                .Select(r => r.Child ?? EntityCache.GetPage(r.ChildId)));
            return orderedChildren;
        }

        // 2) Collect all siblings in sequence
        var visitedChildIds = new HashSet<int>();
        while (currentRelation != null && visitedChildIds.Add(currentRelation.ChildId))
        {
            orderedChildren.Add(currentRelation.Child
                                ?? EntityCache.GetPage(currentRelation.ChildId));

            // Find next sibling exclusively using PreviousId
            currentRelation = parent.ChildRelations
                .FirstOrDefault(r => r.PreviousId == currentRelation.ChildId);
        }

        return orderedChildren;
    }

    public static string ToAsciiParentDiagram(PageCacheItem pageCacheItem)
    {
        var stringBuilder = new StringBuilder();
        var allParentPaths = GetAllParentPaths(pageCacheItem);

        if (allParentPaths.Count == 0)
        {
            stringBuilder.AppendLine(pageCacheItem.Name + " (current - no parents)");
            return stringBuilder.ToString();
        }

        // Display all parent paths
        for (int pathIndex = 0; pathIndex < allParentPaths.Count; pathIndex++)
        {
            var parentPath = allParentPaths[pathIndex];
            
            if (pathIndex > 0)
            {
                stringBuilder.AppendLine(); // Empty line between paths
            }
            
            if (allParentPaths.Count > 1)
            {
                stringBuilder.AppendLine($"Parent Path {pathIndex + 1}:");
            }

            // Display the chain from root to current page
            for (int i = parentPath.Count - 1; i >= 0; i--)
            {
                string indent = new string(' ', (parentPath.Count - 1 - i) * 4);
                stringBuilder.Append(indent);
                
                if (i < parentPath.Count - 1)
                {
                    stringBuilder.Append("└── ");
                }
                
                stringBuilder.AppendLine(parentPath[i].Name);
            }

            // Show the current page at the bottom
            string finalIndent = new string(' ', parentPath.Count * 4);
            stringBuilder.Append(finalIndent);
            stringBuilder.Append("└── ");
            stringBuilder.AppendLine(pageCacheItem.Name + " (current)");
        }

        return stringBuilder.ToString();
    }    /// <summary>
    /// Gets all possible parent paths from the current page up to root pages.
    /// Returns a list of paths, where each path is a list of parent pages from immediate parent to root.
    /// </summary>
    private static List<List<PageCacheItem>> GetAllParentPaths(PageCacheItem page)
    {
        var allPaths = new List<List<PageCacheItem>>();
        var visitedPages = new HashSet<int>();
        
        GetParentPathsRecursive(page, new List<PageCacheItem>(), allPaths, visitedPages);
        
        return allPaths;
    }

    /// <summary>
    /// Recursively builds all possible parent paths to avoid infinite loops in case of circular references.
    /// </summary>
    private static void GetParentPathsRecursive(PageCacheItem currentPage, List<PageCacheItem> currentPath, List<List<PageCacheItem>> allPaths, HashSet<int> visitedPages)
    {
        // Prevent infinite loops
        if (visitedPages.Contains(currentPage.Id))
        {
            return;
        }

        visitedPages.Add(currentPage.Id);

        // If no parents, this is a complete path to root
        if (currentPage.ParentRelations == null || currentPage.ParentRelations.Count == 0)
        {
            allPaths.Add(new List<PageCacheItem>(currentPath));
        }
        else
        {
            // For each parent, continue building the path
            foreach (var parentRelation in currentPage.ParentRelations)
            {
                var parent = parentRelation.Parent ?? EntityCache.GetPage(parentRelation.ParentId);
                
                if (parent != null)
                {
                    var newPath = new List<PageCacheItem>(currentPath) { parent };
                    var newVisited = new HashSet<int>(visitedPages);
                    GetParentPathsRecursive(parent, newPath, allPaths, newVisited);
                }
            }
        }
    }
}