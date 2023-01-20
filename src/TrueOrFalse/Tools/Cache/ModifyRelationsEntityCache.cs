using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Data;

public class ModifyRelationsEntityCache
{
    public static void DeleteIncludetContentOf(CategoryCacheItem category)
    {
        var allParents = GraphService.GetAllParentsFromEntityCache(category.Id);
        foreach (var parent in allParents)
        {
            for (var i = 0; i < parent.CategoryRelations.Count; i++)
            {
                var relation = parent.CategoryRelations[i];

                if (relation.RelatedCategoryId == category.Id)
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
            RelatedCategoryId = parentId,
            CategoryId = child.Id
        }); 
    }
    public static void RemoveRelation(CategoryCacheItem categoryCacheItem, int relatedId)
    {
        for (int i = 0; i < categoryCacheItem.CategoryRelations.Count; i++)
        {
            var relation = categoryCacheItem.CategoryRelations[i];

            if (relation.CategoryId == categoryCacheItem.Id &&
                relation.RelatedCategoryId == relatedId)
            {
                categoryCacheItem.CategoryRelations.RemoveAt(i);
                break;
            }
        }
    }
}