using System.Collections.Generic;
using System.Linq;

public class CategoryNavigationModel : BaseModel
{
    public List<Category> ActiveCategories;
    public Category RootCategory;

    public List<Category> CategoryTrail;

    public List<Category> DefaultCategoriesList = Sl.CategoryRepo.GetDefaultCategoriesList();
    

    public CategoryNavigationModel()
    {
        ActiveCategories = TopicMenu.ActiveCategories;

        if (ActiveCategories != null)
        {
            //HIER KOMMT DIE PFADFINDUNG DER KATEGORIE REIN
            CategoryTrail = GetBreadCrumb.For(ActiveCategories.First()).ToList();
            CategoryTrail.Reverse();
            SetRootCategory();
        }
    }

    private void SetRootCategory()
    {
        if (DefaultCategoriesList.Contains(ActiveCategories.First()))
        {
            RootCategory = ActiveCategories.First();
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