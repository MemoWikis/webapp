public class CrumbtrailItem
{
    public string Text;

    public CrumbtrailItem(Category category)
    {
        Text = category.Name;
    }
}
