

using System.Linq;

class TestHelper
    {
        public static bool hasRelation(Category category, int categoryId, int relatedId)
        {
            return category.CategoryRelations.Any(c =>
                c.Category.Id == categoryId && c.RelatedCategory.Id == relatedId);
        }

        public static bool hasRelation(CategoryCacheItem category, int categoryId, int relatedId)
        {
            return category.CategoryRelations.Any(c =>
                c.CategoryId == categoryId && c.RelatedCategoryId== relatedId);
        }
}

