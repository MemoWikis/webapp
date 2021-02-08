public class CrumbtrailItem
{
    public string Text;
    public readonly Category Category;

    public CrumbtrailItem(Category category)
    {
        Text = category.Name;
        Category = category;
    }

    public bool IsEqual(Category category) => Category.Id == category.Id;
}
