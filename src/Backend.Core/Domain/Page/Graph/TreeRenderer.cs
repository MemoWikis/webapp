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
}