using System;
using System.Collections.Generic;
using System.Linq;

public class Intervalizer
{
    public static IntervalizerResult Run(List<AnswerPair> examinedAnswerObjects, TimeSpan intervalLength)
    {
        var listOfIntervals = new List<IntervalizerResultItem>();
        if (examinedAnswerObjects.Any())
        {
            var maxTimeSpanInSeconds = examinedAnswerObjects.Any() ? examinedAnswerObjects.Last().TimePassedInSeconds : 0;
            var numberOfIntervals = (int)Math.Floor(maxTimeSpanInSeconds / intervalLength.TotalSeconds) + 1;
            for (var i = 0; i < numberOfIntervals; i++)
            {
                listOfIntervals.Add(new IntervalizerResultItem(intervalLength, i));
            }
            examinedAnswerObjects.ForEach(x =>
            {
                var intervalIndex = (int)Math.Floor(x.TimePassedInSeconds / intervalLength.TotalSeconds);
                listOfIntervals[intervalIndex].AddPair(x.ExaminedAnswer, x.NextAnswer);
            });
        }

        return new IntervalizerResult(listOfIntervals);
    }
}
