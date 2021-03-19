using System;
using System.Collections.Generic;

[Serializable]
public class CategoryCachedData
{
    public IList<int> TotalAggregatedChildrenIds { get; set; } = new List<int>();
    public IList<int> ChildrenIds { get; set; } = new List<int>(); 
}
