using System;
using System.Collections.Generic;

[Serializable]
public class CategoryCachedData
{
    public IList<int> TotalAggregatedChildren { get; set; } = new List<int>();
    public IList<int> Children { get; set; } = new List<int>(); 
}
