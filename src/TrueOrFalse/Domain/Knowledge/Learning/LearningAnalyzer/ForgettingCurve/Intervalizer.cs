using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;

public class Intervalizer
{
    public static ForgettingCurve Run(IList<AnswerPair> answerPairs, TimeSpan intervalLength, int maxIntervalCount = 30)
    {
        var listOfIntervals = new List<IntervalizerResultItem>();
        if (!answerPairs.Any())
            return new ForgettingCurve(listOfIntervals);

        var maxTimeSpanInSeconds = answerPairs.Any() ? answerPairs.Last().TimePassedInSeconds : 0;
        var numberOfIntervals = (int)Math.Floor(maxTimeSpanInSeconds / intervalLength.TotalSeconds) + 1;
        numberOfIntervals = numberOfIntervals > maxIntervalCount ? maxIntervalCount : numberOfIntervals;

        for (var i = 0; i < numberOfIntervals; i++)
            listOfIntervals.Add(new IntervalizerResultItem(intervalLength, i));
        
        answerPairs.ForEach(x =>
        {
            var intervalIndex = (int)Math.Floor(x.TimePassedInSeconds / intervalLength.TotalSeconds);
            intervalIndex = intervalIndex >= numberOfIntervals ? numberOfIntervals - 1 : intervalIndex;

            listOfIntervals[intervalIndex].AddPair(x.ExaminedAnswer, x.NextAnswer);
        });

        return new ForgettingCurve(listOfIntervals);
    }
}
