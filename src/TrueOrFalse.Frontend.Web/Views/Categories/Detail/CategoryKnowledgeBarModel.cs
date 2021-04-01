public class CategoryKnowledgeBarModel : BaseModel
{
    public CategoryCacheItem Category;

    public KnowledgeSummary CategoryKnowledgeSummary;

    public CategoryKnowledgeBarModel(CategoryCacheItem category)
    {
        Category = category;
        CategoryKnowledgeSummary = KnowledgeSummaryLoader.RunFromMemoryCache(category.Id, Sl.SessionUser.UserId);
    }
}
