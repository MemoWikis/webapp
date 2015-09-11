using System.Collections.Generic;
using System.Linq;

public class ProbabilityCalc_Question : IRegisterAsInstancePerLifetime
{
    public static int Run(IList<AnswerHistory> answerHistoryItems)
    {
        if (!answerHistoryItems.Any())
            return 30;

        decimal answeredCorrectly = 
            answerHistoryItems.Count(x => x.AnswerredCorrectly != AnswerCorrectness.False);

        return (int)((answeredCorrectly / answerHistoryItems.Count()) * 100);
    }
}