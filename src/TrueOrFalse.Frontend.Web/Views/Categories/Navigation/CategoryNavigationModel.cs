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

    private void ExtractRootCategoryFromTrail(List<Category> categoryTrail)
    {
        if (DefaultCategoriesList.Contains(categoryTrail.First()))
        {
            RootCategory = categoryTrail.First();
            categoryTrail.RemoveAt(0);
            return;
        }

        if (categoryTrail.Count > 0)
        {
            foreach (var category in categoryTrail)
            {
                if (DefaultCategoriesList.Any(d => category.Id == d.Id))
                {
                    RootCategory = category;

                    var rootCategoryIndex = categoryTrail.FindIndex(c => c == category);
                    categoryTrail.RemoveRange(0, rootCategoryIndex);
                    return;
                }
            }
        }

        RootCategory = Sl.CategoryRepo.Allgemeinwissen;
    }

    private void FindActiveCategoryPath(List<Category> actualCategories)
    {

        if (Sl.SessionUiData.TopicMenu.UserCategoryPath.Count > 0)
        {
            var userCategoryPath = Sl.SessionUiData.TopicMenu.UserCategoryPath;
            foreach (var actualCategory in actualCategories)
            {
                var pathIndex = userCategoryPath.FindIndex(c => c == actualCategory);
                if (pathIndex != -1)
                {
                    userCategoryPath.RemoveRange(pathIndex + 1, userCategoryPath.Count - pathIndex + 1); //TODO:Julian Null Pointer Exception if position not exsitent
                    Sl.SessionUiData.TopicMenu.UserCategoryPath = userCategoryPath;

                    var categoryTrail = new List<Category>(userCategoryPath);//TODO:Julian There is a better way to solve this
                    categoryTrail.RemoveAt(categoryTrail.Count - 1); //Remove actualCategory
                    ExtractRootCategoryFromTrail(categoryTrail);
                    CategoryTrail = categoryTrail;

                    ActiveCategory = actualCategory;
                    return;
                }
            }
        }

        if (Sl.SessionUiData.VisitedCategories.Any())
        {
            var lastVisitedCategory = Sl.CategoryRepo.GetById(Sl.SessionUiData.VisitedCategories.First().Id);
            foreach (var actualCategory in actualCategories)
            {
                if (lastVisitedCategory.AggregatedCategories().Contains(actualCategory))
                {
                    var connectingCategoryPath = ThemeMenuHistoryOps.GetConnectingCategoryPath(lastVisitedCategory, actualCategory);
                    Sl.SessionUiData.TopicMenu.UserCategoryPath = connectingCategoryPath;

                    connectingCategoryPath.RemoveAt(0);
                    connectingCategoryPath.RemoveAt(connectingCategoryPath.Count - 1);
                    CategoryTrail = connectingCategoryPath;
                    ActiveCategory = actualCategory;
                    return;
                }
            }
        }

        var activeCategory = actualCategories.First();
        ActiveCategory = activeCategory;
        var categoryPath = new List<Category>(GetBreadCrumb.For(actualCategories.First()));
        categoryPath.Add(activeCategory);
        ExtractRootCategoryFromTrail(categoryPath);
        categoryPath.Insert(0, RootCategory);
        Sl.SessionUiData.TopicMenu.UserCategoryPath = categoryPath;

        categoryPath.RemoveAt(0);
        categoryPath.RemoveAt(categoryPath.Count - 1);
        CategoryTrail = categoryPath; //TODO:Julian There is a better way...
    }
}