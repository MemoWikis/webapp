public class CategoryKnowledgeBarModel
{
    public CategoryCacheItem Category;

    public KnowledgeSummary CategoryKnowledgeSummary;

    public CategoryKnowledgeBarModel(CategoryCacheItem category, int userId)
    {
        Category = category;
        CategoryKnowledgeSummary = KnowledgeSummaryLoader.RunFromMemoryCache(category.Id, userId);
    }
}
