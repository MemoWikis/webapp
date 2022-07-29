using System.Linq;

class TestHelper
{
    public static bool HasRelation(Category category, int categoryId, int relatedId)
    {
        return category.CategoryRelations.Any(c =>
            c.Category.Id == categoryId && c.RelatedCategory.Id == relatedId);
    }

    public static bool HasRelation(CategoryCacheItem category, int categoryId, int relatedId)
    {
        return category.CategoryRelations.Any(c =>
            c.CategoryId == categoryId && c.RelatedCategoryId== relatedId);
    }
}

