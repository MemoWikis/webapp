using System;
using System.Collections.Generic;
using System.Linq;

public class ComparisonTableModel
{
    public IEnumerable<AlgoTesterSummary> Summaries;
    public AlgoTesterSummary Winner;
    public AnswerFeature Feature;

    public bool IsFeatureView { get { return Feature != null; } }

    public string ToggleId;
    public bool ShowCollapsed; 

    public ComparisonTableModel()
    {
        ToggleId = Guid.NewGuid().ToString();
    }

    public ComparisonTableModel(IList<AlgoTesterSummary> summaries) : this()
    {
        if (!summaries.Any())
            return;

        Summaries = summaries;
        Winner = Summaries.OrderByDescending(x => x.SuccessRate).First();
    }

    public ComparisonTableModel(FeatureModel repetitionFeature) : this()
    {
        if (!repetitionFeature.Summaries.Any())
            return;

        Summaries = repetitionFeature.Summaries;
        Feature = repetitionFeature.Feature;
        Winner = Summaries.OrderByDescending(x => x.SuccessRate).First();

        ShowCollapsed = true;
    }
}