using System.Collections.Generic;
using System.Linq;

public class CategoryNavigationModel : BaseModel
{
    public Category ActiveCategory;
    public Category RootCategory;
    private List<Category> _activeCategories;

    public List<Category> CategoryConnectionTrail;

    public List<Category> DefaultCategoriesList = Sl.CategoryRepo.GetDefaultCategoriesList();
    

    public CategoryNavigationModel()
    {
        _activeCategories = TopicMenu.ActiveCategories;

        if (_activeCategories != null)
        {
            FindActiveCategoryPath(_activeCategories);
        }
    }

    private List<Category> ExtractRootCategoryFromPath(List<Category> categoryTrail)
    {
        if (categoryTrail.Count > 0)
        {
            if (DefaultCategoriesList.Contains(categoryTrail.First()))
            {
                RootCategory = categoryTrail.First();
                return categoryTrail;
            }

            foreach (var category in categoryTrail)
            {
                if (DefaultCategoriesList.Any(d => category.Id == d.Id))
                {
                    RootCategory = category;

                    var rootCategoryIndex = categoryTrail.FindIndex(c => c == category);
                    categoryTrail.RemoveRange(0, rootCategoryIndex - 1);
                    return categoryTrail;
                }
            }
        }

        RootCategory = Sl.CategoryRepo.Allgemeinwissen;
        categoryTrail.Insert(0, RootCategory);
        return categoryTrail;
    }

    private void FindActiveCategoryPath(List<Category> actualCategories)
    {
        List<Category> categoryConnectionTrail;

        var userCategoryPath = Sl.SessionUiData.TopicMenu.UserCategoryPath;
        if (userCategoryPath?.Count > 0)
        {
            foreach (var actualCategory in actualCategories)
            {
                var pathIndex = userCategoryPath.FindIndex(c => c == actualCategory);
                if (pathIndex != -1)
                {
                    if(userCategoryPath.Count > pathIndex + 1)
                        userCategoryPath.RemoveRange(pathIndex + 1, userCategoryPath.Count - (pathIndex + 1)); //TODO:Julian Null Pointer Exception if position not exsitent
                    Sl.SessionUiData.TopicMenu.UserCategoryPath = userCategoryPath;

                    categoryConnectionTrail = new List<Category>(userCategoryPath);
                    ExtractRootCategoryFromPath(categoryConnectionTrail);
                    categoryConnectionTrail.RemoveAt(0);
                    if (categoryConnectionTrail.Count > 0)
                        categoryConnectionTrail.RemoveAt(categoryConnectionTrail.Count - 1);
                    CategoryConnectionTrail = categoryConnectionTrail;

                    ActiveCategory = actualCategory;
                    return;
                }
            }

            var lastVisitedCategory = userCategoryPath.Last();
            var lastVisitedCategoryAggregatedCategories = lastVisitedCategory.AggregatedCategories(false);
            foreach (var actualCategory in actualCategories)
            {
                if (lastVisitedCategoryAggregatedCategories.Contains(actualCategory))
                {
                    var connectingCategoryPath = ThemeMenuHistoryOps.GetConnectedCategoryPath(new List<Category> { lastVisitedCategory }, actualCategory);
                    ExtractRootCategoryFromPath(connectingCategoryPath);
                    Sl.SessionUiData.TopicMenu.UserCategoryPath = connectingCategoryPath;

                    categoryConnectionTrail = new List<Category>(connectingCategoryPath);
                    categoryConnectionTrail.RemoveAt(0);
                    if(categoryConnectionTrail.Count > 0)
                        categoryConnectionTrail.RemoveAt(categoryConnectionTrail.Count - 1);
                    CategoryConnectionTrail = categoryConnectionTrail;
                    ActiveCategory = actualCategory;
                    return;
                }
            }
        }

        ActiveCategory = actualCategories.First();
        var categoryPath = new List<Category>(GetBreadCrumb.For(actualCategories.First()));
        categoryPath.Add(ActiveCategory);
        categoryPath = ExtractRootCategoryFromPath(categoryPath);
        Sl.SessionUiData.TopicMenu.UserCategoryPath = categoryPath;

        categoryConnectionTrail = new List<Category>(categoryPath);
        categoryConnectionTrail.RemoveAt(0);
        if(categoryConnectionTrail.Count > 0)
            categoryConnectionTrail.RemoveAt(categoryConnectionTrail.Count - 1);
        CategoryConnectionTrail = categoryConnectionTrail;
    }
}