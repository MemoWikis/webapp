using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class AlgoTester
{
	public static void Run()
	{
        var stopWatch = new Stopwatch();
        Logg.r().Information("AlgoTester Start");

		var allAnswerHistoryItems = Sl.R<AnswerRepo>().GetAll().OrderBy(x => x.Id);
		var allPreviousItems = new List<Answer>();

		var answerHistoryTestRepo = Sl.R<AnswerTestRepo>();

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
        List<Answer> allPreviousItems, 
        Answer answerItem, 
        AnswerTestRepo answerTestRepo, 
        AlgoInfo algo)
	{
	    var question = answerItem.GetQuestion();
	    var user = answerItem.GetUser();

        var result = algo.Algorithm.Run(question, user, allPreviousItems);

		var answerHistoryTest = new AnswerTest
		{
			Answer = answerItem,
			AlgoId = algo.Id,
			Probability = result.Probability,
			IsCorrect = answerItem.AnsweredCorrectly()
				? result.Probability > 50
				: result.Probability <= 50
		};

		answerTestRepo.Create(answerHistoryTest);
	}
}