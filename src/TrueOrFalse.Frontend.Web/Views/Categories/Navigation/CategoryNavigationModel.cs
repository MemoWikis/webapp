using System.Collections.Generic;
using System.Linq;

public class CategoryNavigationModel : BaseModel
{
    public Category ActualCategory;
    public Category RootCategory;

    public List<Category> CategoryTrail;

    public List<Category> DefaultCategoriesList = Sl.CategoryRepo.GetDefaultCategoriesList();
    public Category Allgemeinwissen = Sl.CategoryRepo.GetDefaultCategoriesList().First(c => c.Id == 709);

    public CategoryNavigationModel()
    {
        ActualCategory = ThemeMenu.ActualCategory;
        if (ActualCategory != null)
        {
            CategoryTrail = GetBreadCrumb.For(ActualCategory).ToList();
            CategoryTrail.Reverse();
            SetRootCategory();
        }
    }

    private void SetRootCategory()
    {
        if (DefaultCategoriesList.Contains(ActualCategory))
        {
            RootCategory = ActualCategory;
            return;
        }

        if (CategoryTrail.Count > 0)
        {
            foreach (var category in CategoryTrail)
            {
                if (DefaultCategoriesList.Any(d => category.Id == d.Id))
                {
                    RootCategory = category;

                    var rootCategoryIndex = CategoryTrail.FindIndex(c => c == category);
                    CategoryTrail.RemoveRange(rootCategoryIndex, CategoryTrail.Count - rootCategoryIndex);

                    return;
                }
            }
        }

        RootCategory = Allgemeinwissen;
    }
}