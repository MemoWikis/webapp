[Serializable]
public class CategoryCacheRelation
{
    public virtual int CategoryId { get; set; }
    public virtual int ParentCategoryId { get; set; }

    public IList<CategoryCacheRelation> ToListCategoryRelations(IList<CategoryRelation> listCategoryRelations)
    {
        var result = new List<CategoryCacheRelation>();

        if (listCategoryRelations == null)
            Logg.r.Error("CategoryRelations cannot be null");

        if (listCategoryRelations.Count <= 0 || listCategoryRelations == null)
        {
            return result;
        }
        foreach (var categoryRelation in listCategoryRelations)
        {
            result.Add(ToCategoryCacheRelation(categoryRelation));
        }

        return result;
    }

    public static CategoryCacheRelation ToCategoryCacheRelation(CategoryRelation categoryRelation)
    {
        return new CategoryCacheRelation
        {
            CategoryId = categoryRelation.Child.Id,
            ParentCategoryId = categoryRelation.Parent.Id
        };
    }

    public static bool IsCategorRelationEqual(CategoryCacheRelation relation1, CategoryCacheRelation relation2)
    {
        return relation1.ParentCategoryId == relation2.ParentCategoryId &&
               relation1.CategoryId == relation2.CategoryId;
    }
}