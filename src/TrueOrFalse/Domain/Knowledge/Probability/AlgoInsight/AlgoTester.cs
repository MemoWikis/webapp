using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class AlgoTester
{
	public static void Run()
	{
        var stopWatch = new Stopwatch();
        Logg.r().Information("AlgoTester Start");

		var allAnswer = Sl.R<AnswerRepo>().GetAll().OrderBy(x => x.Id);
		var allPreviousItems = new List<Answer>();

		var answerTestRepo = Sl.R<AnswerTestRepo>();

		var algos = AlgoInfoRepo.GetAll();

		var index = 0;
        foreach (var answerItem in allAnswer)
        {
            foreach(var algo in algos)
                CreateHistoryItem(allPreviousItems, answerItem, answerTestRepo, algo);

            if (index % 10 == 0)
                answerTestRepo.Flush();

            allPreviousItems.Add(answerItem);

            index++;
        }

		Logg.r().Information("AlgoTester End {duration}", stopWatch.Elapsed);
    }

	private static void CreateHistoryItem(
        List<Answer> allPreviousItems, 
        Answer answer, 
        AnswerTestRepo answerTestRepo, 
        AlgoInfo algo)
	{
	    var question = EntityCache.GetQuestion(answer.Question.Id);
	    var user = EntityCache.GetUserById(answer.UserId);

        var result = algo.Algorithm.Run(question, user, allPreviousItems);

		var answerTest = new AnswerTest
		{
			Answer = answer,
			AlgoId = algo.Id,
			Probability = result.Probability,
			IsCorrect = answer.AnsweredCorrectly()
				? result.Probability > 50
				: result.Probability <= 50
		};

		answerTestRepo.Create(answerTest);
	}
}