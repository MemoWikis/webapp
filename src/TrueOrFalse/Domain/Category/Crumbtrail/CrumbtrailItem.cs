public class CrumbtrailItem(CategoryCacheItem _category)
{
    public string Text = _category.Name;
    public readonly CategoryCacheItem Category = _category;

    public bool IsEqual(CategoryCacheItem category) => Category.Id == category.Id;
}
