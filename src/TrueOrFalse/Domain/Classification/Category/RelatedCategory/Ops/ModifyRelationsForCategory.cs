using System.Collections.Generic;
using System.Linq;

public class ModifyRelationsForCategory
{
    public static void UpdateCategoryRelationsOfType(Category category, IList<Category> relatedCategories, CategoryRelationType relationType)
    {
        var existingRelationsOfType = category.CategoryRelations.Any()
            ? category.CategoryRelations?.Where(r => r.CategoryRelationType == relationType).ToList()
            : new List<CategoryRelation>();

        var relationsToAdd = relatedCategories
            .Except(existingRelationsOfType.Select(r => r.RelatedCategory))
            .Select(c => new CategoryRelation{Category = category, RelatedCategory = c, CategoryRelationType = relationType});

        var relationsToRemove = existingRelationsOfType.Where(r => relatedCategories.All(c => c != r.RelatedCategory)).ToList();

        foreach (var relation in relationsToAdd)
            category.CategoryRelations.Add(relation);

        foreach (var relation in relationsToRemove)
            category.CategoryRelations.Remove(relation);
    }

    public static void AddCategoryRelationOfType(Category category, Category relatedCategory,CategoryRelationType relationType)
    {
        
    }
}
