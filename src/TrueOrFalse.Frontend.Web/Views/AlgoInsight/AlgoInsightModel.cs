using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;
using TrueOrFalse.Web;

public class AlgoInsightModel : BaseModel
{
    public UIMessage Message;
    public IEnumerable<AlgoTesterSummary> Summaries;

    public IEnumerable<FeatureModel> FeatureModelsRepetition;
    public IEnumerable<FeatureModel> FeatureModelsTime;
    public IEnumerable<FeatureModel> FeatureModelsTrainingType;

    public AlgoInsightModel()
    {
        Summaries = AlgoTesterSummaryLoader.Run().OrderByDescending(x => x.SuccessRate);

        var allFeatures = Sl.R<AnswerFeatureRepo>().GetAll();
        var featureSummaries = AlgoTesterSummaryLoader.RunWithFeature();

        FeatureModelsRepetition = GetFeatureModels(featureSummaries, allFeatures, AnswerFeatureGroups.Repetitions);
        FeatureModelsTime = GetFeatureModels(featureSummaries, allFeatures, AnswerFeatureGroups.Time);
        FeatureModelsTrainingType = GetFeatureModels(featureSummaries, allFeatures, AnswerFeatureGroups.TrainingType);
    }

    private IEnumerable<FeatureModel> GetFeatureModels(
        IEnumerable<AlgoTesterSummary> featureSummaries,
        IEnumerable<AnswerFeature> allFeatures,
        string groupName)
    {
        return allFeatures
            .Where(x => x.Group == groupName)
            .Select(x =>
                new FeatureModel
                {
                    Feature = x,
                    Summaries = featureSummaries.ByFeature(x.Name)
                });
    }

    public string GetBubbleChartRows(IEnumerable<FeatureModel> featureModels)
    {
        var result = "[['ID', 'Anzahl Wiederholungen', 'Algorithmus-Vorhersagegenauigkeit %', 'Algo', 'Anzahl Antworten'],";

        featureModels
            .ForEach(x => x.Summaries
            .ForEach(y => result +=
                String.Format("['', {0}, {1}, '{2}', {3}],",
                    GetRepetitionCount(y.FeatureName),
                    y.SuccessRateInPercent,
                    y.Algo.Name,
                    y.TestCount)));
        
        return result + "]";
    }

    public string GetRepetitionCount(string featureName)
    {
        return featureName.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Last().Trim();
    }
}