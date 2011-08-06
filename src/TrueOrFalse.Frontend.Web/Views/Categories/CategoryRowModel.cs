using TrueOrFalse.Core;

public class CategoryRowModel
{
    public CategoryRowModel(Category category)
    {
        CategoryName = category.Name;
        CategoryId = category.Id;
    }

    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
}