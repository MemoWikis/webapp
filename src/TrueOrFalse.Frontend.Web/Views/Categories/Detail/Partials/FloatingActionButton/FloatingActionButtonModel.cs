public class FloatingActionButtonModel : BaseContentModule
{
    public CategoryCacheItem Category;
    public bool IsTopicTab;

    public FloatingActionButtonModel(CategoryCacheItem categoryCacheCategory, bool isTopicTab)
    {
        Category = categoryCacheCategory;
        IsTopicTab = isTopicTab;
    }
}