using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueOrFalse;

public class ProbabilityForAllUsersCalc : IRegisterAsInstancePerLifetime
{
    public int Run(IList<AnswerHistory> answerHistoryItems)
    {
        if (!answerHistoryItems.Any())
            return 30;

        decimal answeredCorrectly = answerHistoryItems.Count(x => x.AnswerredCorrectly);
        return (int)((answeredCorrectly / answerHistoryItems.Count()) * 100);
    }
}