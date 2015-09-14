using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class AlgoTester
{
	public static void Run()
	{
        var stopWatch = new Stopwatch();
        Logg.r().Information("AlgoTester Start");

		var allAnswerHistoryItems = Sl.R<AnswerHistoryRepo>().GetAll().OrderBy(x => x.Id);
		var allPreviousItems = new List<AnswerHistory>();

		var answerHistoryTestRepo = Sl.R<AnswerHistoryTestRepo>();

		var algos = AlgoInfoRepo.GetAll();

		var index = 0;
        foreach (var answerHistoryItem in allAnswerHistoryItems)
        {
            foreach(var algo in algos)
                CreateHistoryItem(allPreviousItems, answerHistoryItem, answerHistoryTestRepo, algo);

            if (index % 10 == 0)
                answerHistoryTestRepo.Flush();

            allPreviousItems.Add(answerHistoryItem);

            index++;
        }

		Logg.r().Information("AlgoTester End {duration}", stopWatch.Elapsed);
    }

	private static void CreateHistoryItem(
        List<AnswerHistory> allPreviousItems, 
        AnswerHistory answerHistoryItem, 
        AnswerHistoryTestRepo answerHistoryTestRepo, 
        AlgoInfo algo)
	{
	    var question = answerHistoryItem.GetQuestion();
	    var user = answerHistoryItem.GetUser();

        var result = algo.Algorithm.Run(question, user, allPreviousItems);

		var answerHistoryTest = new AnswerHistoryTest
		{
			AnswerHistory = answerHistoryItem,
			AlgoId = algo.Id,
			Probability = result.Probability,
			IsCorrect = answerHistoryItem.AnsweredCorrectly()
				? result.Probability > 50
				: result.Probability <= 50
		};

		answerHistoryTestRepo.Create(answerHistoryTest);
	}
}