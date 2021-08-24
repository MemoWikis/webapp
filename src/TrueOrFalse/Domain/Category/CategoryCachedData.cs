using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class CategoryCachedData
{
    public IList<int> TotalAggregatedChildrenIds { get; set; } = new List<int>();
    public IList<int> ChildrenIds  { get; set; } = new List<int>();

    public int CountVisibleChildrenIds =>
        EntityCache.GetCategoryCacheItems(ChildrenIds).Count(cci => cci.IsVisibleToCurrentUser());

    public int CountAllChildrenIds => ChildrenIds.Count; 
}
