
public class ModifyRelationsEntityCache
{
    public static void RemoveRelations(CategoryCacheItem category)
    {
        var allParents = GraphService.GetAllParentsFromEntityCache(category.Id);
        foreach (var parent in allParents)
        {
            for (var i = 0; i < parent.CategoryRelations.Count; i++)
            {
                var relation = parent.CategoryRelations[i];

                if (relation.ParentCategoryId == category.Id)
                {
                    parent.CategoryRelations.Remove(relation);
                    break;
                }
            }
        }
    }

    public static void AddParent(CategoryCacheItem child, int parentId)
    {
        child.CategoryRelations.Add(new CategoryCacheRelation
        {
            ParentCategoryId = parentId,
            ChildCategoryId = child.Id
        }); 
    }
    public static void RemoveRelation(CategoryCacheItem categoryCacheItem, int relatedId)
    {
        for (int i = 0; i < categoryCacheItem.CategoryRelations.Count; i++)
        {
            var relation = categoryCacheItem.CategoryRelations[i];

            if (relation.ChildCategoryId == categoryCacheItem.Id &&
                relation.ParentCategoryId == relatedId)
            {
                categoryCacheItem.CategoryRelations.RemoveAt(i);
                break;
            }
        }
    }
}