using System.Collections.Generic;
using System.Linq;

public class CategoryNavigationModel : BaseModel
{
    public Category ActiveCategory;
    public Category RootCategory;

    public List<Category> CategoryTrail;

    public List<Category> RootCategoriesList = Sl.CategoryRepo.GetRootCategoriesList();
    

    public CategoryNavigationModel()
    {
        var activeCategories = TopicMenu.PageCategories;
        if (activeCategories != null)
        {
            SetCategoryPath(activeCategories);
        }
    }

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

    private void SetCategoryPath(IList<Category> pageCategories)
    {
        List<Category> categoryTrail;

        var userCategoryPath = Sl.SessionUiData.TopicMenu.CategoryPath.ToList();
        if (userCategoryPath?.Count > 0)
        {
            foreach (var category in pageCategories)
            {
                var pathIndex = userCategoryPath.FindIndex(c => c == category);
                if (pathIndex != -1)
                {
                    if(userCategoryPath.Count > pathIndex + 1)
                        userCategoryPath.RemoveRange(pathIndex + 1, userCategoryPath.Count - (pathIndex + 1));

                    Sl.SessionUiData.TopicMenu.CategoryPath = userCategoryPath;

                    categoryTrail = new List<Category>(userCategoryPath);
                    GetRootCategoryFromPath(categoryTrail);
                    categoryTrail.RemoveAt(0);
                    if (categoryTrail.Count > 0)
                        categoryTrail.RemoveAt(categoryTrail.Count - 1);

                    CategoryTrail = categoryTrail;
                    ActiveCategory = category;

                    return;
                }
            }

            var lastVisitedCategoryId = userCategoryPath.Last().Id;
            var lastVisitedCategory = Sl.CategoryRepo.GetById(lastVisitedCategoryId);
            var lastVisitedCategoryAggregatedCategories = lastVisitedCategory.AggregatedCategories(false);
            foreach (var actualCategory in pageCategories)
            {
                if (lastVisitedCategoryAggregatedCategories.Contains(actualCategory))
                {
                    var connectingCategoryPath = ThemeMenuHistoryOps.GetConnectedCategoryPath(new List<Category> { actualCategory }, lastVisitedCategory);
                    userCategoryPath.RemoveAt(userCategoryPath.Count - 1);
                    connectingCategoryPath.InsertRange(0, userCategoryPath);
                    GetRootCategoryFromPath(connectingCategoryPath);
                    Sl.SessionUiData.TopicMenu.CategoryPath = connectingCategoryPath;

                    categoryTrail = new List<Category>(connectingCategoryPath);
                    categoryTrail.RemoveAt(0);
                    if(categoryTrail.Count > 0)
                        categoryTrail.RemoveAt(categoryTrail.Count - 1);
                    CategoryTrail = categoryTrail;
                    ActiveCategory = actualCategory;
                    return;
                }
            }
        }

        ActiveCategory = pageCategories.FirstOrDefault();
        var categoryPath = new List<Category>(GetBreadCrumb.For(pageCategories.FirstOrDefault()));

        if(ActiveCategory != null) 
            categoryPath.Add(ActiveCategory);

        categoryPath = GetRootCategoryFromPath(categoryPath);

        Sl.SessionUiData.TopicMenu.CategoryPath = categoryPath;

        categoryTrail = new List<Category>(categoryPath);
        categoryTrail.RemoveAt(0);
        if(categoryTrail.Count > 0)
            categoryTrail.RemoveAt(categoryTrail.Count - 1);

        CategoryTrail = categoryTrail;
    }
}