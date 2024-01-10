using System.Linq;

class TestHelper
{
    public static bool HasParent(Category category, int relatedId)
    {
        return category.CategoryRelations.Any(c =>
            c.Child.Id == category.Id && c.Parent.Id == relatedId);
    }

    public static bool HasParent(CategoryCacheItem category, int relatedId)
    {
        return category.CategoryRelations.Any(c =>
            c.CategoryId == category.Id && c.RelatedCategoryId== relatedId);
    }
}

