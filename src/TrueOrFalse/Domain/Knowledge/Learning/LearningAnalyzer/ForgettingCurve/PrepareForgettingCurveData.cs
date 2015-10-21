using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PrepareForgettingCurveData
{
    public static List<TimeIntervalWithAnswers> Run(List<AnswerHistory> answerHistories, TimeSpan intervalLength)
    {
        var answerHistoryRows = GetAnswerHistoryRows.Run(answerHistories);
        var listOfExaminedAnswerObjects = GetExaminedAnswerObjects.Run(answerHistoryRows).OrderBy(x => x.TimePassedInSeconds).ToList();
        return Intervalizer.Run(listOfExaminedAnswerObjects, intervalLength);
    }
}
