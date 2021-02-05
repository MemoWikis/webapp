using System;
using System.Collections.Generic;

[Serializable]
public class CategoryCachedData
{
    public IList<Category> TotalAggregatedChildren { get; set; }
    public IList<Category> Children { get; set; }
}
