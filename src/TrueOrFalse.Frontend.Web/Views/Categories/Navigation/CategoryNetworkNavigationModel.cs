using System.Collections.Generic;

public class CategoryNetworkNavigationModel : BaseModel
{

    public string CategoryName;
    public int CategoryId;
    public IList<Category> CategoriesParent;
    public IList<Category> CategoriesChildren;

    public CategoryNetworkNavigationModel(int categoryId)
    {
        var category = R<CategoryRepository>().GetById(categoryId);
        CategoryName = category.Name;
        CategoryId = category.Id;
        CategoriesParent = category.ParentCategories();
        CategoriesChildren = R<CategoryRepository>().GetChildren(category.Id);
    }


}