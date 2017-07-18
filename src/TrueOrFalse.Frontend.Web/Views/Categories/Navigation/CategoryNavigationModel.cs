using System.Collections.Generic;
using System.Linq;

public class CategoryNavigationModel : BaseModel
{
    public Category ActiveCategory;
    public Category RootCategory;

    public List<Category> CategoryTrail;

    public List<Category> DefaultCategoriesList = Sl.CategoryRepo.GetDefaultCategoriesList();
    

    public CategoryNavigationModel()
    {
        ActiveCategory = TopicMenu.ActiveCategory;
        if (ActiveCategory != null)
        {
            CategoryTrail = GetBreadCrumb.For(ActiveCategory).ToList();
            CategoryTrail.Reverse();
            SetRootCategory();
        }
    }

    private void SetRootCategory()
    {
        if (DefaultCategoriesList.Contains(ActiveCategory))
        {
            RootCategory = ActiveCategory;
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

        RootCategory = Sl.CategoryRepo.Allgemeinwissen;
    }
}