using System;
using System.Collections.Generic;
using System.Linq;

public class PrepareForgettingCurveData
{
    public static List<IntervalizerResultItem> Run(List<AnswerHistory> answerHistories, TimeSpan intervalLength)
    {
        var answerHistoryPairs = GetAnswerHistoryPairs.Run(answerHistories);
        var listOfExaminedAnswerObjects = answerHistoryPairs.OrderBy(x => x.TimePassedInSeconds).ToList();
        return Intervalizer.Run(listOfExaminedAnswerObjects, intervalLength);
    }
}
