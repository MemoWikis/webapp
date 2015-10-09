using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;

public class TabForecastModel
{
    public IEnumerable<AlgoTesterSummary> Summaries;

    public IEnumerable<FeatureModel> FeatureModelsRepetition;
    public IEnumerable<FeatureModel> FeatureModelsTime;
    public IEnumerable<FeatureModel> FeatureModelsTrainingType;

    public IEnumerable<AlgoTesterSummary> FeatureSummaries;
    public IEnumerable<AlgoTesterSummary> TopFeatures;

    public TabForecastModel()
    {
        Summaries = AlgoTesterSummaryLoader.Run().OrderByDescending(x => x.SuccessRate);

        var allFeatures = Sl.R<AnswerFeatureRepo>().GetAll();
        FeatureSummaries = AlgoTesterSummaryLoader.RunWithFeature();
        TopFeatures = FeatureSummaries
            .Where(x => x.TestCount > 50)
            .OrderByDescending(x => x.SuccessRate)
            .GroupBy(x => x.FeatureName)
            .Select(x => x.First());

        FeatureModelsRepetition = GetFeatureModels(allFeatures, AnswerFeatureGroups.Repetitions);
        FeatureModelsTime = GetFeatureModels(allFeatures, AnswerFeatureGroups.Time);
        FeatureModelsTrainingType = GetFeatureModels(allFeatures, AnswerFeatureGroups.TrainingType);
    }

    private IEnumerable<FeatureModel> GetFeatureModels(
        IEnumerable<AnswerFeature> allFeatures,
        string groupName)
    {
        return allFeatures
            .Where(x => x.Group == groupName)
            .Select(x =>
                new FeatureModel
                {
                    Feature = x,
                    Summaries = FeatureSummaries.ByFeature(x.Name)
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