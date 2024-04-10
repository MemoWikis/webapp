public class CategoryKnowledgeBarModel
{
    public CategoryCacheItem Category;
    public KnowledgeSummary CategoryKnowledgeSummary;

    public CategoryKnowledgeBarModel(CategoryCacheItem category, int userId, KnowledgeSummaryLoader knowledgeSummaryLoader)
    {
        Category = category;
        CategoryKnowledgeSummary = knowledgeSummaryLoader.RunFromMemoryCache(category.Id, userId);
    }
}
