public class CrumbtrailItem
{
    public string Text;
    public readonly CategoryCacheItem Category;

    public CrumbtrailItem(CategoryCacheItem category)
    {
        Text = category.Name;
        Category = category;
    }

    public bool IsEqual(CategoryCacheItem category) => Category.Id == category.Id;
}
