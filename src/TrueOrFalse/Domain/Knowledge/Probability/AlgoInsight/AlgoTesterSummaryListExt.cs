using System.Collections.Generic;
using System.Linq;

public static class AlgoTesterSummaryListExt
{
    public static IEnumerable<AlgoTesterSummary> ByGroup(this IEnumerable<AlgoTesterSummary> summaries, string featureGroup)
    {
        return summaries.Where(x => x.FeatureGroup == featureGroup);
    }

    public static IEnumerable<AlgoTesterSummary> ByFeature(this IEnumerable<AlgoTesterSummary> summaries, string featureName)
    {
        return summaries.Where(x => x.FeatureName == featureName);
    }
}