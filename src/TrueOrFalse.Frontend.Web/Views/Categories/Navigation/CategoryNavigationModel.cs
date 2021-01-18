using System.Collections.Generic;
using System.Linq;

public class CategoryNavigationModel : BaseModel
{
    public Category ActiveCategory;
    public Category RootCategory;

    public List<Category> CategoryTrail;

    public List<Category> RootCategoriesList = Sl.CategoryRepo.GetRootCategoriesList();
    

    private List<Category> GetRootCategoryFromPath(List<Category> categoryPath)
    {
        if (categoryPath.Count > 0)
        {
            if (RootCategoriesList.Contains(categoryPath.First()))
            {
                RootCategory = categoryPath.First();
                return categoryPath;
            }

            foreach (var category in categoryPath)
            {
                if (RootCategoriesList.Any(d => category.Id == d.Id))
                {
                    RootCategory = category;

                    var rootCategoryIndex = categoryPath.FindIndex(c => c == category);
                    categoryPath.RemoveRange(0, rootCategoryIndex - 1);
                    return categoryPath;
                }
            }
        }

        RootCategory = Sl.CategoryRepo.Allgemeinwissen;
        categoryPath.Insert(0, RootCategory);

        return categoryPath;
    }
}