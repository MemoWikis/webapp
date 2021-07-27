using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

public class SegmentationModel : BaseContentModule
{
    public CategoryCacheItem Category;

    public string Title;
    public string Text;
    public bool HasCustomSegments = false;

    public List<CategoryCacheItem> CategoryList;
    public List<CategoryCacheItem> NotInSegmentCategoryList;
    public List<Segment> Segments;
    public string NotInSegmentCategoryIds;
    public string SegmentJson;
    public bool IsMyWorld { get; set; }

    public SegmentationModel(CategoryCacheItem category)
    {
        IsMyWorld = UserCache.GetItem(Sl.CurrentUserId).IsFiltered;
        Category = category;
        
        var categoryList = UserCache.GetItem(_sessionUser.UserId).IsFiltered ? UserEntityCache.GetChildren(category.Id, UserId).Where(c => c.IsVisibleToCurrentUser()) : EntityCache.GetChildren(category.Id).Where(c => c.IsVisibleToCurrentUser());
        CategoryList = categoryList.Where(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Standard).ToList();

        var segments = new List<Segment>();
        if (!String.IsNullOrEmpty(category.CustomSegments) && !UserCache.GetItem(_sessionUser.UserId).IsFiltered)
        {
            segments = GetSegments(category.Id);
            NotInSegmentCategoryList = GetNotInSegmentCategoryList(segments, categoryList.ToList());
            Segments = segments;
        }
        else
            NotInSegmentCategoryList = categoryList.OrderBy(c => c.Name).ToList();

        var childCategoryIds = NotInSegmentCategoryList.GetIds();
        NotInSegmentCategoryIds = "[" + String.Join(",", childCategoryIds) + "]";
        if (Segments != null)
        {
            var filteredSegments = Segments.Select(s => new
            {
                CategoryId = s.Item.Id,
                Title = s.Title,
                ChildCategoryIds = "[" + String.Join(", ", s.ChildCategories.Select(c => c.Id).ToList()) + "]",
            }).ToList(); ;
            SegmentJson = HttpUtility.HtmlEncode(JsonConvert.SerializeObject(filteredSegments));
        }

    }


    public List<Segment> GetSegments(int id)
    {
        var segments = new List<Segment>();
        var segmentJson = JsonConvert.DeserializeObject<List<SegmentJson>>(EntityCache.GetCategoryCacheItem(id).CustomSegments);
        foreach (var s in segmentJson)
        {
            var segment = new Segment();
            var segmentItem = EntityCache.GetCategoryCacheItem(s.CategoryId);
            if (segmentItem.IsNotVisibleToCurrentUser)
                continue;
            segment.Item = EntityCache.GetCategoryCacheItem(s.CategoryId);
            segment.Title = String.IsNullOrEmpty(s.Title) ? segment.Item.Name : s.Title;

            var childCategories = new List<CategoryCacheItem>();
                
            if (s.ChildCategoryIds != null)
                childCategories = UserCache.GetItem(_sessionUser.UserId).IsFiltered
                    ? EntityCache.GetCategoryCacheItems(s.ChildCategoryIds).Where(c => c.IsInWishknowledge() && c.IsVisibleToCurrentUser()).ToList()
                    : EntityCache.GetCategoryCacheItems(s.ChildCategoryIds).Where(c => c.IsVisibleToCurrentUser()).ToList();
            else
                childCategories = UserCache.GetItem(_sessionUser.UserId).IsFiltered ? 
                    UserEntityCache.GetChildren(s.CategoryId, UserId).Where(c => c.IsVisibleToCurrentUser()).ToList() : 
                    EntityCache.GetChildren(s.CategoryId).Where(c => c.IsVisibleToCurrentUser()).ToList();
            segment.ChildCategories = childCategories;
            segments.Add(segment);
        }

        return segments.Distinct().OrderBy(s => s.Title).ToList();
    }

    public List<CategoryCacheItem> GetNotInSegmentCategoryList(List<Segment> segments, List<CategoryCacheItem> categoryList)
    {
        var notInSegmentCategoryList = new List<CategoryCacheItem>();
        var inSegmentCategoryList = new List<CategoryCacheItem>();

        foreach (var segment in segments)
        {

            inSegmentCategoryList.Add(segment.Item);
            if (segment.ChildCategories != null)
            {
                var categoriesToAdd = segment.ChildCategories.Where(c => !inSegmentCategoryList.Any(s => s.Id == c.Id)).ToList();
                foreach (var c in categoriesToAdd)
                    inSegmentCategoryList.Add(c);
            }

            notInSegmentCategoryList.AddRange(categoryList.Where(c => !inSegmentCategoryList.Any(s => c.Id == s.Id) && !notInSegmentCategoryList.Any(s => s.Id == c.Id)));
        }

        HasCustomSegments = true;
        return notInSegmentCategoryList.OrderBy(c => c.Name).ToList();
    }
}






