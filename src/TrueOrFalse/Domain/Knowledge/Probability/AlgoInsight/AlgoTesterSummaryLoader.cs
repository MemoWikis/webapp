using System.Collections.Generic;
using System.Linq;

public class AlgoTesterSummaryLoader
{
    public static List<AlgoTesterSummary> Run()
    {
        var result = new List<AlgoTesterSummary>();
        var allAlgos = AlgoInfoRepo.GetAll();

        var loadSummaries = Sl.R<AnswerHistoryTestRepo>().LoadSummaries();

        foreach (var currentSummary in loadSummaries)
        {
            var algoSummary = new AlgoTesterSummary(currentSummary);
            algoSummary.Algo = allAlgos.First(a => a.Id == currentSummary.AlgoId);

            result.Add(algoSummary);
        }

        return result;
    }
}