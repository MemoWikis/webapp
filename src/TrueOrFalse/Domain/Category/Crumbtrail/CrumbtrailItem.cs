public class CrumbtrailItem
{
    public string Text;
    private Category _category;

    public CrumbtrailItem(Category category)
    {
        Text = category.Name;
        _category = category;
    }

    public bool IsEqual(Category category) => _category.Id == category.Id;
}
