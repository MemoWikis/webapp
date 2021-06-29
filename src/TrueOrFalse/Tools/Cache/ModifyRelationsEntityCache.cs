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

                if (relation.RelatedCategoryId == category.Id && relation.CategoryRelationType == CategoryRelationType.IncludesContentOf)
                    parent.CategoryRelations.Remove(relation);
            }
        }
    }

    public static IEnumerable<CategoryCacheRelation> GetRelationsToAdd(
        CategoryCacheItem categoryCacheItem,
        IEnumerable<Category> relatedCategoriesAsCategories,
        CategoryRelationType relationType,
        IEnumerable<CategoryRelation> existingRelationsOfType)
    {
        return relatedCategoriesAsCategories
            .Except(existingRelationsOfType.Select(r => r.RelatedCategory))
            .Select(c => new CategoryCacheRelation
                {
                    CategoryId = categoryCacheItem.Id,
                    RelatedCategoryId = c.Id,
                    CategoryRelationType = relationType
                }
            );
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

    public static void CreateIncludeContentOf(CategoryCacheItem category, IEnumerable<CategoryCacheRelation> relationsToAdd)
    {

        foreach (var relation in relationsToAdd)
        {
            category.CategoryRelations.Add(relation);
        }
    }
}