using System;
using System.Collections.Generic;
using System.Linq;

public class SegmentationModel : BaseContentModule
{
    public Category Category;

    public string Title;
    public string Text;

    public List<Category> CategoryList;
    public List<Category> UnsortedCategoryList;
    public List<Segment> Segments;

    public SegmentationModel()
    {
    }

    public SegmentationModel(Category category)
    {
        Category = category;
        
        var categoryList = UserCache.IsFiltered ? UserEntityCache.GetChildren(category.Id, UserId) : EntityCache.GetChildren(category.Id).ToList();
        CategoryList = categoryList.Where(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Standard).ToList();

        var sortedCategories = new List<Category>();
        if (!IsLoggedIn)
        {
            if (Segments != null)
                foreach (var segment in Segments)
                {
                    var categoriesToAdd = segment.CategoryList.Where(c => !sortedCategories.Any(s => s.Id == c.Id));
                    foreach (var c in categoriesToAdd)
                       sortedCategories.Add(c);
                }       

            UnsortedCategoryList = categoryList.Where(c => !sortedCategories.Any(s => c.Id == s.Id)).ToList();
        } else
            UnsortedCategoryList = categoryList;
    }
}






