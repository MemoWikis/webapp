using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using NHibernate.Mapping;

public class SegmentModel : BaseContentModule
{
    public string Title;
    public CategoryCacheItem Category;
    public List<CategoryCacheItem> ChildCategories;
    public string ChildCategoryIds;

    public SegmentModel(Segment segment)
    {
        Category = segment.Item;
        var childCategories = segment.ChildCategories;
        Title = segment.Title ?? segment.Item.Name;

        ChildCategories = childCategories;
        var childCategoryIds = new List<int>();
        childCategories.ForEach(c => childCategoryIds.Add(c.Id));
        ChildCategoryIds = "[" + String.Join(",", childCategoryIds) + "]";
    }
}






