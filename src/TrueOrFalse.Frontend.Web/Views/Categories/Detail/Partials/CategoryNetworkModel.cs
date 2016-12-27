using System.Collections.Generic;
using System.Linq;

public class CategoryNetworkModel : BaseModel
{
    public int Id;
    public string Name;

    public IList<Category> CategoriesParent;
    public IList<Category> CategoriesChildren;

    public Category Category;

    private readonly CategoryRepository _categoryRepo;

    public CategoryNetworkModel(Category category)
    {
        _categoryRepo = R<CategoryRepository>();

        Category = category;

        Id = category.Id;
        Name = category.Name;

        CategoriesParent = category.ParentCategories;
        CategoriesChildren = _categoryRepo.GetChildren(category.Id);
    }
}