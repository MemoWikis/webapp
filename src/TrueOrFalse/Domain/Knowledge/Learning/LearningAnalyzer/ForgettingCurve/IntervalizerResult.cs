using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

[DebuggerDisplay("Intervals {Items.Count}")]
public class IntervalizerResult
{
    public int TotalPairs; 
    public IList<IntervalizerResultItem> Items;

    public IntervalizerResult(IList<IntervalizerResultItem> items)
    {
        TotalPairs = items.Sum(x => x.NumberOfPairs);
        Items = items;
    }
}