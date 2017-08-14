public class CategoryKnowledgeBarModel : BaseModel
{
    public Category Category;

    public KnowledgeSummary CategoryKnowledgeSummary;

    public CategoryKnowledgeBarModel(Category category)
    {
        Category = category;
        CategoryKnowledgeSummary = KnowledgeSummaryLoader.RunFromMemoryCache(category, Sl.SessionUser.UserId);
    }
}
