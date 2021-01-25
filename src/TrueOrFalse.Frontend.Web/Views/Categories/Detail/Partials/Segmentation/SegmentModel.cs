using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using NHibernate.Mapping;

public class SegmentModel : BaseContentModule
{
    public string Title;
    public Category Category;
    public List<Category> ChildCategories;

    public SegmentModel(Segment segment)
    {
        Category = segment.Category;
        ChildCategories = segment.ChildCategories;
        if (segment.Title != null)
            Title = segment.Title;
        else
            Title = segment.Category.Name;
    }
}






