
public class ModifyRelationsEntityCache
{
    public static void RemoveRelations(CategoryCacheItem category)
    {
        var allParents = GraphService.Ascendants(category.Id);
        foreach (var parent in allParents)
        {
            for (var i = 0; i < parent.ParentRelations.Count; i++)
            {
                var relation = parent.ParentRelations[i];

                if (relation.ParentId == category.Id)
                {
                    parent.ParentRelations.Remove(relation);
                    break;
                }
            }
        }
    }

    public static void AddParent(int childId, int parentId) => AddParent(EntityCache.GetCategory(childId), parentId);

    public static void AddParent(CategoryCacheItem child, int parentId)
    {
        child.ParentRelations.Add(new CategoryCacheRelation
        {
            ParentId = parentId,
            ChildId = child.Id
        }); 
    }
    public static void RemoveParent(CategoryCacheItem categoryCacheItem, int parentId)
    {
        for (int i = 0; i < categoryCacheItem.ParentRelations.Count; i++)
        {
            var relation = categoryCacheItem.ParentRelations[i];

            if (relation.ChildId == categoryCacheItem.Id &&
                relation.ParentId == parentId)
            {
                categoryCacheItem.ParentRelations.RemoveAt(i);
                break;
            }
        }
    }

    public static void MoveTopic(int childId, int oldParentId, int newParentId)
    {
        var child = EntityCache.GetCategory(childId);
        AddParent(child, newParentId);
        RemoveParent(child, oldParentId);
    }
}