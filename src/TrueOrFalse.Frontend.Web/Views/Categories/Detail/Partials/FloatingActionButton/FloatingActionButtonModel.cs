public class FloatingActionButtonModel : BaseContentModule
{
    public Category Category;
    public bool IsTopicTab;

    public FloatingActionButtonModel(Category category, bool isTopicTab)
    {
        Category = category;
        IsTopicTab = isTopicTab;
    }
}