using TrueOrFalse.Core;

public class CategoryRowModel
{
    public CategoryRowModel(Category category)
    {
        CategoryName = category.Name;
    }

    public string CategoryName { get; set; }
}