

using System;
using System.Collections.Generic;
using NHibernate.Mapping;

public class CategoryCacheRelations
{
    public virtual int CategoryId { get; set; }

    public virtual int RelatedCategoryId { get; set; }

    public virtual CategoryRelationType CategoryRelationType { get; set; }

    public IList<CategoryCacheRelations> ToListCategoryRelations(
        IList<CategoryRelation> listCategoryRelations)
    {
        var result = new List<CategoryCacheRelations>();

        if (listCategoryRelations == null)
            Logg.r().Error("CategoryRelations cannot be null");

        foreach (var categoryRelation in listCategoryRelations)
        {
            result.Add(ToUserEntityCacheRelation(categoryRelation));
        }

        return result;
    }

    public CategoryCacheRelations ToUserEntityCacheRelation(CategoryRelation categoryRelation)
    {
        return new CategoryCacheRelations
        {
            CategoryId = categoryRelation.Category.Id,
            CategoryRelationType = categoryRelation.CategoryRelationType,
            RelatedCategoryId = categoryRelation.RelatedCategory.Id
        };

    }
}

