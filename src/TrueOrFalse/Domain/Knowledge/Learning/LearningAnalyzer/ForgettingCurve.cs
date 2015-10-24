using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

[DebuggerDisplay("Intervals {Intervals.Count}")]
public class ForgettingCurve
{
    public int TotalPairs; 
    public IList<IntervalizerResultItem> Intervals;

    public ForgettingCurve(IList<IntervalizerResultItem> intervals)
    {
        TotalPairs = intervals.Sum(x => x.NumberOfPairs);
        Intervals = intervals;
    }
}