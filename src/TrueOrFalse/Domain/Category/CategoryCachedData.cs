using System;
using System.Collections.Generic;

[Serializable]
public class CategoryCachedData
{
    public IList<Category> TotalAggregatedChildren { get; set; } = new List<Category>();
    public IList<Category> Children { get; set; } = new List<Category>(); 
}
