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

                if (relation.RelatedCategoryId == category.Id &&
                    relation.CategoryRelationType == CategoryRelationType.IncludesContentOf)
                {
                    parent.CategoryRelations.Remove(relation);
                    break;
                }
            }
        }
    }

    public static void AddParent(
        CategoryCacheItem child, int parentId)
    {
        child.CategoryRelations.Add(new CategoryCacheRelation
        {
            RelatedCategoryId = parentId,
            CategoryRelationType = CategoryRelationType.IsChildOf,
            CategoryId = child.Id
        }); 

        EntityCache.GetCategoryCacheItem(parentId).CategoryRelations.Add(
      new CategoryCacheRelation 
      {
          CategoryId = parentId,
          CategoryRelationType = CategoryRelationType.IncludesContentOf,
          RelatedCategoryId = child.Id

      });
    }
    public static void RemoveRelation(CategoryCacheItem categoryCacheItem, int relatedId, CategoryRelationType categoryRelationType)
    {
        for (int i = 0; i < categoryCacheItem.CategoryRelations.Count; i++)
        {
            var relation = categoryCacheItem.CategoryRelations[i];

            if (relation.CategoryId == categoryCacheItem.Id &&
                relation.RelatedCategoryId == relatedId &&
                relation.CategoryRelationType == categoryRelationType)
            {
                categoryCacheItem.CategoryRelations.RemoveAt(i);
                break;
            }
        }
    }
}