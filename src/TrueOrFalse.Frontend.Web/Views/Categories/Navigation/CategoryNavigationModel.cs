using System.Collections.Generic;
using System.Linq;

public class CategoryNavigationModel : BaseModel
{
    public Category ActiveCategory;
    public Category RootCategory;
    private List<Category> _activeCategories;

    public List<Category> CategoryTrail;

    public List<Category> DefaultCategoriesList = Sl.CategoryRepo.GetDefaultCategoriesList();
    

    public CategoryNavigationModel()
    {
        _activeCategories = TopicMenu.ActiveCategories;

        if (_activeCategories != null)
        {
            FindActiveCategoryPath(_activeCategories);
        }
    }

    private List<Category> ExtractRootCategoryFromTrail(List<Category> categoryTrail)
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

                    var categoryTrail = new List<Category>(userCategoryPath); //TODO:Julian There is a better way to solve this
                    ExtractRootCategoryFromTrail(categoryTrail);
                    categoryTrail.RemoveAt(0);
                    if (categoryTrail.Count > 0)
                        categoryTrail.RemoveAt(categoryTrail.Count - 1);
                    CategoryTrail = categoryTrail;

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
                    ExtractRootCategoryFromTrail(connectingCategoryPath);
                    connectingCategoryPath.Insert(0, RootCategory);
                    Sl.SessionUiData.TopicMenu.UserCategoryPath = connectingCategoryPath;

                    connectingCategoryPath.RemoveAt(0);
                    connectingCategoryPath.RemoveAt(connectingCategoryPath.Count - 1);
                    CategoryTrail = connectingCategoryPath;
                    ActiveCategory = actualCategory;
                    return;
                }
            }
        }

        ActiveCategory = actualCategories.First();
        var categoryPath = new List<Category>(GetBreadCrumb.For(actualCategories.First()));
        categoryPath.Add(ActiveCategory);
        categoryPath = ExtractRootCategoryFromTrail(categoryPath); //TODO:Julian Error possible because of Allgemeinwissen added to Path (= wrong Path)
        Sl.SessionUiData.TopicMenu.UserCategoryPath = new List<Category>(categoryPath);

        categoryPath.RemoveAt(0);
        if(categoryPath.Count > 0)
            categoryPath.RemoveAt(categoryPath.Count - 1);
        CategoryTrail = categoryPath;
    }
}