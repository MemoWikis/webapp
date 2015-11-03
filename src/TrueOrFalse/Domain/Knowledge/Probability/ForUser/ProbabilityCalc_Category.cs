using System.Collections.Generic;
using System.Linq;

public class ProbabilityCalc_User
{
    public static int Run(IList<Answer> answerHistoryItems)
    {
        if (!answerHistoryItems.Any())
            return 50;

        decimal answeredCorrectly =
            answerHistoryItems.Count(x => x.AnswerredCorrectly != AnswerCorrectness.False);

        return (int)((answeredCorrectly / answerHistoryItems.Count()) * 100);
    }
}