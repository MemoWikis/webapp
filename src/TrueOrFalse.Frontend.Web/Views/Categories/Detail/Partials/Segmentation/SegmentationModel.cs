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

        var segments = new List<Segment>();
        if (category.CustomSegments != null)
        {
            segments = GetSegments(category.Id);
            NotInSegmentCategoryList = GetNotInSegmentCategoryList(segments, categoryList);
            Segments = segments;
        }
        else
            NotInSegmentCategoryList = categoryList.OrderBy(c => c.Name).ToList();
    }

    public List<Segment> GetSegments(int id)
    {
        var segments = new List<Segment>();
        var segmentJson = JsonConvert.DeserializeObject<List<SegmentJson>>(EntityCache.GetCategory(id).CustomSegments);
        foreach (var s in segmentJson)
        {
            var segment = new Segment();
            segment.Category = EntityCache.GetCategory(s.CategoryId);
            segment.Title = s.Title;
            if (s.ChildCategoryIds != null)
                segment.ChildCategories = UserCache.IsFiltered ? EntityCache.GetCategories(s.ChildCategoryIds).Where(c => c.IsInWishknowledge()).ToList() : EntityCache.GetCategories(s.ChildCategoryIds).ToList();
            else
                segment.ChildCategories = UserCache.IsFiltered ? UserEntityCache.GetChildren(s.CategoryId, UserId) : Sl.CategoryRepo.GetChildren(s.CategoryId).ToList();

            segments.Add(segment);
        }

        return segments.OrderBy(s => s.Title).ToList();
    }

    public List<Category> GetNotInSegmentCategoryList(List<Segment> segments, List<Category> categoryList)
    {
        var notInSegmentCategoryList = new List<Category>();
        var inSegmentCategoryList = new List<Category>();

        foreach (var segment in segments)
        {
            inSegmentCategoryList.Add(segment.Category);
            if (segment.ChildCategories != null)
            {
                var categoriesToAdd = segment.ChildCategories.Where(c => !inSegmentCategoryList.Any(s => s.Id == c.Id)).ToList();
                foreach (var c in categoriesToAdd)
                    inSegmentCategoryList.Add(c);
            }

            notInSegmentCategoryList.AddRange(categoryList.Where(c => !inSegmentCategoryList.Any(s => c.Id == s.Id)));
        }

        HasCustomSegments = true;
        return notInSegmentCategoryList.OrderBy(c => c.Name).ToList();
    }
}






