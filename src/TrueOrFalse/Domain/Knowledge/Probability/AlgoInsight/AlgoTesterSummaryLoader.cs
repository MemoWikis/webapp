using System.Collections.Generic;
using System.Linq;

public class AlgoTesterSummaryLoader
{
    public static List<AlgoTesterSummary> Run()
    {
        return GetSummaries(Sl.R<AnswerTestRepo>().LoadSummaries());
    }

    public static List<AlgoTesterSummary> RunWithFeature()
    {
        return GetSummaries(Sl.R<AnswerTestRepo>().LoadSummariesWithFeatures());
    }

    private static List<AlgoTesterSummary> GetSummaries(IList<AlgoSummary> summaries)
    {
        var result = new List<AlgoTesterSummary>();

        var allAlgos = AlgoInfoRepo.GetAll();

        foreach (var currentSummary in summaries)
        {
            var algoSummary = new AlgoTesterSummary(currentSummary);
            algoSummary.Algo = allAlgos.First(a => a.Id == currentSummary.AlgoId);

            result.Add(algoSummary);
        }

        return result;
    }

}