using System;
using System.Collections.Generic;
using System.Diagnostics;
using Seedworks.Lib.Persistence;

[DebuggerDisplay("{Category.Name}({CategoryId.Id}) {RelatedCategoryId.Name}({RelatedCategory.Id})")]
[Serializable]
public class CategoryRelation : DomainEntity
{
    public virtual Category Category { get; set; }

    public virtual Category RelatedCategory { get; set; }
    
    public virtual IList<CategoryRelation> ToListCategoryRelations(
        IList<CategoryCacheRelation> listCategoryRelations)
    {
        var result = new List<CategoryRelation>();

        if (listCategoryRelations == null)
            Logg.r().Error("CategoryRelations cannot be null");

        foreach (var categoryRelation in listCategoryRelations)
        {
            result.Add(FromEntityCacheRelation(categoryRelation));
        }

        return result;
    }

    public virtual CategoryRelation FromEntityCacheRelation(CategoryCacheRelation categoryRelation)
    {
        return new CategoryRelation
        {
            Category = Category.ToCategory(EntityCache.GetCategory(categoryRelation.CategoryId)),
            RelatedCategory = Category.ToCategory(EntityCache.GetCategory(categoryRelation.RelatedCategoryId))
        };
    }
}
