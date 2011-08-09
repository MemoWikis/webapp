using TrueOrFalse.Core;

public class SubCategoryItemRowModel    
{

    public SubCategoryItemRowModel(string name)
    {
        Name = name; 
    }

    public SubCategoryItemRowModel(SubCategoryItem item)
    {
        Name = item.Name;
    }

    public string Name { get; set; }
}