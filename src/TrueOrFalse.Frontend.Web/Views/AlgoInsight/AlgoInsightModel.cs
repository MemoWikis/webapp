using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Web;

public class AlgoInsightModel : BaseModel
{
    public UIMessage Message;
    public IEnumerable<AlgoTesterSummary> Summaries;
    public IEnumerable<FeatureModel> RepetitionFeatureModels;

    public AlgoInsightModel()
    {
        Summaries = AlgoTesterSummaryLoader.Run().OrderByDescending(x => x.SuccessRate);

        var allFeatures = Sl.R<AnswerFeatureRepo>().GetAll();
        var featureSummaries = AlgoTesterSummaryLoader.RunWithFeature();

        RepetitionFeatureModels = 
            allFeatures
                .Where(x => x.Group == AnswerFeatureGroups.Repetitions)
                .Select(x => 
                    new FeatureModel{
                        Feature = x,
                        Summaries = featureSummaries.ByFeature(x.Name)
                    });
    }
}