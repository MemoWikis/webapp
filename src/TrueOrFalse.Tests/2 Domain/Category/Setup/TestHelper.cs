

using System.Linq;

class TestHelper
    {
        public static bool hasRelation(Category category, int categoryId, int relatedId, CategoryRelationType categoryRelationType)
        {
            return category.CategoryRelations.Any(c =>
                c.Category.Id == categoryId && c.RelatedCategory.Id == relatedId &&
                c.CategoryRelationType == categoryRelationType);
        }

        public static bool hasRelation(CategoryCacheItem category, int categoryId, int relatedId, CategoryRelationType categoryRelationType)
        {
            return category.CategoryRelations.Any(c =>
                c.CategoryId == categoryId && c.RelatedCategoryId== relatedId &&
                c.CategoryRelationType == categoryRelationType);
        }
}

