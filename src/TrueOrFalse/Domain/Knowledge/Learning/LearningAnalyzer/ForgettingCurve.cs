using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

[DebuggerDisplay("Intervals {Intervals.Count} TimeSpanLength {TimeSpanLength}")]
public class ForgettingCurve
{
    public int TotalPairs;
    public int TotalIntervals { get { return Intervals.Count; } }

    public IList<IntervalizerResultItem> Intervals;

    public TimeSpan IntervalsTimeSpan{ get { return Intervals.Last().TimePassedUpperBound; } }
    public TimeSpan TimeSpanLength { get { return Intervals.First().TimeIntervalLength; } }

    public ForgettingCurve(IList<IntervalizerResultItem> intervals)
    {
        TotalPairs = intervals.Sum(x => x.NumberOfPairs);
        Intervals = intervals;
    }
}