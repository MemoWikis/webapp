

using System;
using System.Collections.Generic;


[Serializable]
public class CategoryCacheRelation
{
    public virtual int CategoryId { get; set; }

    public virtual int RelatedCategoryId { get; set; }

    public virtual CategoryRelationType CategoryRelationType { get; set; }

    public IList<CategoryCacheRelation> ToListCategoryRelations(
        IList<CategoryRelation> listCategoryRelations)
    {
        var result = new List<CategoryCacheRelation>();

        if (listCategoryRelations == null)
            Logg.r().Error("CategoryRelations cannot be null");

        foreach (var categoryRelation in listCategoryRelations)
        {
            result.Add(ToUserEntityCacheRelation(categoryRelation));
        }

        return result;
    }

    public CategoryCacheRelation ToUserEntityCacheRelation(CategoryRelation categoryRelation)
    {
        return new CategoryCacheRelation
        {
            CategoryId = categoryRelation.Category.Id,
            CategoryRelationType = categoryRelation.CategoryRelationType,
            RelatedCategoryId = categoryRelation.RelatedCategory.Id
        };

    }
}

