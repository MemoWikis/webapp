using System;
using System.Linq;

public class ForgettingCurveJson
{
    public static object Load()
    {
        return new {};
    }

    public static object GetSampleAll()
    {
        var forgettingCurve = ForgettingCurveLoader.GetForAll(ForgettingCurveInterval.Week, 200);

        return new
        {
            Data = new
            {
                cols = new[]
                {
                    new {id = "Time", label = "Time", type = "number"},
                    new {id = "Alle", label = "Alle Fragen", type = "number"},
                },
                rows = forgettingCurve.Intervals.Where(x => x.NumberOfPairs > 5).Select(x =>
                {
                    var week = Math.Round(x.TimePassedUpperBound.TotalDays / 7, 0);
                    var percentage = Math.Round(x.ProportionAnsweredCorrect*100, 0);

                    return new
                    {
                        c = new[]
                        {
                            new
                            {
                                v = week,
                                f = "Woche: " + week.ToString() + ""
                            },
                            new
                            {
                                v = percentage,
                                f = String.Format("Wahrscheinlichkeit: {0}%,  (Antworten: {1})", percentage, x.NumberOfPairs)
                            },
                        }
                    };
                }

                ).ToArray()
            }

        };
    }

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