using System;
using System.Collections.Generic;
using System.Linq;

public class ForgettingCurveLoader
{
    public static ForgettingCurve Get(List<AnswerHistory> answerHistories, TimeSpan intervalLength)
    {
        var answerHistoryPairs = AnswerPairFromHistoryRows.Get(answerHistories);
        var listOfExaminedAnswerObjects = answerHistoryPairs.OrderBy(x => x.TimePassedInSeconds).ToList();
        return Intervalizer.Run(listOfExaminedAnswerObjects, intervalLength);
    }
}