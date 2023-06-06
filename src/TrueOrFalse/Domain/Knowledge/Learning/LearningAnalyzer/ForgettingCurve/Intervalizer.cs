using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;

public class Intervalizer
{
    public static ForgettingCurve Run(IList<AnswerPair> answerPairs, TimeSpan intervalLength, int intervalCount = 30)
    {
        var listOfIntervals = new List<IntervalizerResultItem>();

        for (var i = 0; i < intervalCount ; i++)
            listOfIntervals.Add(new IntervalizerResultItem(intervalLength, i));

        if (!answerPairs.Any())
            return new ForgettingCurve(listOfIntervals);

        answerPairs.ForEach(x =>
        {
            var intervalIndex = (int)Math.Floor(x.TimePassedInSeconds / intervalLength.TotalSeconds);
            intervalIndex = intervalIndex >= intervalCount ? intervalCount - 1 : intervalIndex;

            listOfIntervals[intervalIndex].AddPair(x.ExaminedAnswer, x.NextAnswer);
        });

        return new ForgettingCurve(listOfIntervals);
    }
}
