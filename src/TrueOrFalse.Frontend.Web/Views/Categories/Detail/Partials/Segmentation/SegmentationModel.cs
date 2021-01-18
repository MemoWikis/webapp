using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping;

public class SegmentationModel : BaseContentModule
{
    public Category Category;

    public string Title;
    public string Text;
    public bool HasCustomSegments = false;

    public List<Category> CategoryList;
    public List<Category> NotInSegmentCategoryList;
    public List<Segment> Segments;

    public SegmentationModel()
    {
    }

    public SegmentationModel(Category category)
    {
        Category = category;
        
        var categoryList = UserCache.IsFiltered ? UserEntityCache.GetChildren(category.Id, UserId) : EntityCache.GetChildren(category.Id).ToList();
        CategoryList = categoryList.Where(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Standard).ToList();

        var inSegmentCategoryList = new List<Category>();

        if (Segments != null)
        {
            var notInSegmentCategoryList = new List<Category>();
            foreach (var segment in Segments)
            {
                var categoriesToAdd = segment.CategoryList.Where(c => !inSegmentCategoryList.Any(s => s.Id == c.Id));
                foreach (var c in categoriesToAdd)
                    inSegmentCategoryList.Add(c);
                notInSegmentCategoryList.AddRange(categoryList.Where(c => !inSegmentCategoryList.Any(s => c.Id == s.Id)));
            }

            NotInSegmentCategoryList = notInSegmentCategoryList.ToList();
            HasCustomSegments = true;
        } else
            NotInSegmentCategoryList = categoryList;
    }
}






