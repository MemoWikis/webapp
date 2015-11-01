using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;

public class ForgettingCurveJson
{
    private const int MINIMUM_AMOUNT_OF_PAIRS = 3;

    public static object Load(CurvesJsonCmd curvesJsonCmd)
    {
        var intervalType = curvesJsonCmd.Interval.ToForgettingCurveInterval();

        var cols = new List<object> { new { id = "Time", label = "Time", type = "number" } };

        curvesJsonCmd.Process();
        curvesJsonCmd.Curves.ForEach(c => cols.Add(new { id = c.ColumnId, label = c.ColumnLabel, type = "number" }) );

        var forgettingCurves = curvesJsonCmd.Curves.Select(x => x.LoadForgettingCurve(intervalType, curvesJsonCmd.IntervalCount)).ToList();

        forgettingCurves
            .Where(curve => curve.Intervals.All(interval => interval.NumberOfPairs < MINIMUM_AMOUNT_OF_PAIRS))
            .ForEach(curve => {
                curve.Intervals[0].ProportionAnsweredCorrect = 0;
                curve.Intervals[0].NumberOfPairs = 5;
            });

        var intervals = forgettingCurves.First().Intervals;
        var rows = intervals.Select((x, i) =>
        {
            var numberOfInterval = x.NumberOfInterval(intervalType);

            var cols2 = new List<object> {new{
                v = numberOfInterval,
                f = intervalType.InGerman() + ": " + numberOfInterval.ToString() + ""
            }};

            foreach (var curve in forgettingCurves)
            {
                var interval = curve.Intervals[i];
                var percentage = Math.Round(interval.ProportionAnsweredCorrect * 100, 0);

                if (interval.NumberOfPairs < MINIMUM_AMOUNT_OF_PAIRS)
                    cols2.Add(new {});
                else
                    cols2.Add(new
                    {
                        v = percentage,
                        f = String.Format("Wahrscheinlichkeit: {0}%,  (Antworten: {1})", percentage, interval.NumberOfPairs)
                    });
            }

            return new {c = cols2};

        }).ToArray();

        return new
        {
            Data = new
            {
                ChartInfos = forgettingCurves.Select(curve => new
                {
                    TotalPairs = curve.TotalPairs,
                    RegressionValue = "0"
                }),
                ChartData = new
                {
                    cols = cols,
                    rows = rows
                }
            }
        };
    }
}