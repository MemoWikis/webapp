using System;
using System.Collections.Generic;
using System.Linq;

public class ForgettingCurveJson
{
    public static object Load(ForgettingCurveInterval intervalType, CurvesJsonCmd curvesJsonCmd)
    {
        var cols = new List<object> { new { id = "Time", label = "Time", type = "number" } };

        curvesJsonCmd.Process();
        curvesJsonCmd.Curves.ForEach(c => cols.Add(new { id = c.ColumnId, label = c.ColumnLabel, type = "number" }) );

        var forgettingCurves = curvesJsonCmd.Curves.Select(x => x.LoadForgettingCurve(intervalType, curvesJsonCmd.IntervalCount)).ToList();
        foreach (var curve in forgettingCurves.Where(c => c.TotalPairs == 0))
        {
            curve.Intervals[0].ProportionAnsweredCorrect = 0;
            curve.Intervals[0].NumberOfPairs = 5;
        }

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

                if (interval.NumberOfPairs < 5)
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
                cols = cols,
                rows = rows
            }
        };
    }

    /// <summary>exists only for reference</summary>
    public static object GetSample()
    {
        return new
        {
            Data =
                new
                {
                    cols = new[]
                    {
                        new {id = "X", label = "X", type = "number"},
                        new {id = "Leichte", label = "Blau", type = "number"},
                        new {id = "Schwer", label = "Schwer", type = "number"},
                        new {id = "Mittel", label = "Mittel", type = "number"},
                        new {id = "Nobrainer", label = "Nobrainer", type = "number"},
                    },
                    rows = new[]
                    {
                        new {c = new[] {new {v = "1"}, new {v = "10"}, new {v = "10"}, new {v = "10"}, new {v = "10"}}},
                        new {c = new[] {new {v = "2"}, new {v = "10"}, new {v = "9"}, new {v = "8"}, new {v = "7"}}},
                        new {c = new[] {new {v = "3"}, new {v = "10"}, new {v = "8"}, new {v = "6"}, new {v = "4"}}},
                        new {c = new[] {new {v = "4"}, new {v = "10"}, new {v = "7"}, new {v = "6"}, new {v = "3"}}},
                        new {c = new[] {new {v = "5"}, new {v = "10"}, new {v = "6"}, new {v = "6"}, new {v = "3"}}},
                        new {c = new[] {new {v = "6"}, new {v = "1"}, new {v = "5"}, new {v = "6"}, new {v = "3"}}},
                    }
                }
        };
    }
}