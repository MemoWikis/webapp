using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ModifyRelationsEntityCache
{
    public static void DeleteIncludetContentOf(CategoryCacheItem category)
    {
        var allParents = GraphService.GetAllParentsFromEntityCache(category.Id, true);
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
}