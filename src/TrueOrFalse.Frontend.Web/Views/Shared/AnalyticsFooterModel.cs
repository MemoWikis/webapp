using System.Collections.Generic;
using System.Linq;
public class AnalyticsFooterModel
{
    public string CategoryName;
    public IList<CategoryCacheItem> AllParents;
    public int ChildrenCount;
    public string ParentList;
    public int CategoryId;
    public bool IsQuestionssite; 

    public AnalyticsFooterModel(CategoryCacheItem category, bool isQuestionsSite = false, bool isCategoryNull = false)
    {
        IsQuestionssite = isQuestionsSite;

        if (!IsQuestionssite)
            return;

        CategoryName = category.Name;
        CategoryId = category.Id; 
        SetCategoryRelations(isCategoryNull, category);
    }

    public void SetCategoryRelations(bool isCategoryChangeData, CategoryCacheItem category)
    {
        var children = isCategoryChangeData ? new List<CategoryCacheItem>() : GetCategoryChildren.WithAppliedRules(category);
        ChildrenCount = children.Count;
        AllParents = isCategoryChangeData ? new List<CategoryCacheItem>() : GraphService.GetAllParentsFromEntityCache(CategoryId);

        if (AllParents.Count > 0)
           ParentList = GetCategoryParentList(AllParents);
    }


    private string GetCategoryParentList(IList<CategoryCacheItem> allParents)
    {
       string categoryList = "";
       string parentList;
       foreach (var category in allParents.Take(3))
       {
           categoryList = categoryList + category.Name + ", ";
       }
       if (allParents.Count > 3)
       {
           parentList = categoryList + "...";
       }
       else
       {
           categoryList = categoryList.Remove(categoryList.Length - 2);
           parentList = categoryList;
       }

       return "(" + parentList + ")";
    }
}