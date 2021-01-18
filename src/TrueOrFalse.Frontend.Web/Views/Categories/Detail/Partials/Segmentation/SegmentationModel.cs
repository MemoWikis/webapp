using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
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
        var segments = GetSegments(category.Id);

        if (segments != null)
            NotInSegmentCategoryList = GetNotInSegmentCategoryList(segments, categoryList);
        else
            NotInSegmentCategoryList = categoryList;

        Segments = segments;
    }

    public List<Segment> GetSegments(int id)
    {
        var segments = new List<Segment>();
        var segmentJson = JsonConvert.DeserializeObject<List<SegmentJson>>(EntityCache.GetCategory(id).CustomSegments);
        foreach (var s in segmentJson)
        {
            var segment = new Segment();
            segment.Title = s.Title;
            foreach (var categoryId in s.CategoryIds)
            {
                var category = EntityCache.GetCategory(categoryId);
                segment.CategoryList.Add(category);
            }

            segments.Add(segment);
        }

        return segments;
    }

    public List<Category> GetNotInSegmentCategoryList(List<Segment> segments, List<Category> categoryList)
    {
        var notInSegmentCategoryList = new List<Category>();
        var inSegmentCategoryList = new List<Category>();

        foreach (var segment in segments)
        {
            var categoriesToAdd = segment.CategoryList.Where(c => !inSegmentCategoryList.Any(s => s.Id == c.Id));
            foreach (var c in categoriesToAdd)
                inSegmentCategoryList.Add(c);
            notInSegmentCategoryList.AddRange(categoryList.Where(c => !inSegmentCategoryList.Any(s => c.Id == s.Id)));
        }

        HasCustomSegments = true;
        return notInSegmentCategoryList.ToList();
    }
}






