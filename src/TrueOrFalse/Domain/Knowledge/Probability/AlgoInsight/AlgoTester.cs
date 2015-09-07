public class AlgoTester
{
	public static void Run()
	{
		var allAnswerHistoryItems = Sl.R<AnswerHistoryRepository>().GetAll();

		foreach (var answerHistormyItem in allAnswerHistoryItems)
		{
			
		}
	}
}