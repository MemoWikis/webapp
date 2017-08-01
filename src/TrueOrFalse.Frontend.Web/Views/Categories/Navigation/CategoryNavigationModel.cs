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
            FindActiveCategoryPath(ActiveCategories);

            //THIS IS GOING TO BE REMOVED
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

    private void FindActiveCategoryPath(List<Category> actualCategories)
    {

        var userCategoryPath = Sl.SessionUiData.TopicMenu.UserCategoryPath;
        if (userCategoryPath.Count > 0)
        {
            foreach (var actualCategory in actualCategories)
            {
                var pathIndex = userCategoryPath.FindIndex(c => c.Equals(actualCategory));
                if (pathIndex != -1)
                {
                    userCategoryPath.RemoveRange(pathIndex + 1, userCategoryPath.Count - (pathIndex + 1));
                    CategoryTrail = userCategoryPath;
                    return;
                }
            }
        }

        var lastCategory = Sl.CategoryRepo.GetById(Sl.SessionUiData.VisitedCategories.First().Id);
        if (lastCategory != null)
        {
            foreach (var actualCategory in actualCategories)
            {
                if (lastCategory.AggregatedCategories().Contains(actualCategory))
                {
                    //Do the Find Route Stuff
                    return;
                }
            }
        }

        //Do the Default Stuff
    }
}