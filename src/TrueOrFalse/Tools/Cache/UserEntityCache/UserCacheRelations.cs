

using System;
using System.Collections.Generic;
using NHibernate.Mapping;

public class UserCacheRelations
{
    public virtual int CategoryId { get; set; }

    public virtual int RelatedCategoryId { get; set; }

    public virtual CategoryRelationType CategoryRelationType { get; set; }

    public IList<UserCacheRelations> ToListCategoryRelations(
        IList<CategoryRelation> listCategoryRelations)
    {
        var result = new List<UserCacheRelations>();

        if (listCategoryRelations == null)
            Logg.r().Error("CategoryRelations cannot be null");

        foreach (var categoryRelation in listCategoryRelations)
        {
            result.Add(ToUserEntityCacheRelation(categoryRelation));
        }

        return result;
    }

    public UserCacheRelations ToUserEntityCacheRelation(CategoryRelation categoryRelation)
    {
        return new UserCacheRelations
        {
            CategoryId = categoryRelation.Category.Id,
            CategoryRelationType = categoryRelation.CategoryRelationType,
            RelatedCategoryId = categoryRelation.RelatedCategory.Id
        };

    }
}

