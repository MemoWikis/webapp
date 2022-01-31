using System;
using System.Collections.Generic;
using System.Diagnostics;
using Seedworks.Lib.Persistence;

[DebuggerDisplay("{Category.Name}({CategoryId.Id}) [{CategoryRelationType.ToString()}] {RelatedCategoryId.Name}({RelatedCategory.Id})")]
[Serializable]
public class CategoryRelation : DomainEntity
{
    public virtual Category Category { get; set; }

    public virtual Category RelatedCategory { get; set; }

    public virtual CategoryRelationType CategoryRelationType { get; set; }


    public virtual IList<CategoryRelation> ToListCategoryRelations(
        IList<CategoryCacheRelation> listCategoryRelations)
    {
        var result = new List<CategoryRelation>();

        if (listCategoryRelations == null)
            Logg.r().Error("CategoryRelations cannot be null");

        foreach (var categoryRelation in listCategoryRelations)
        {
            result.Add(ToUserEntityCacheRelation(categoryRelation));
        }

        return result;
    }

    public virtual CategoryRelation ToUserEntityCacheRelation(CategoryCacheRelation categoryRelation)
    {
        return new CategoryRelation
        {
            Category = Category.ToCategory(EntityCache.GetCategoryCacheItem(categoryRelation.CategoryId)),
            CategoryRelationType = categoryRelation.CategoryRelationType,
            RelatedCategory = Category.ToCategory(EntityCache.GetCategoryCacheItem(categoryRelation.RelatedCategoryId))
        };
    }
}
