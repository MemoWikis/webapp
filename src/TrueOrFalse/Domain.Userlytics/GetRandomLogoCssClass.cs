using System;
using System.Collections.Generic;

public class GetRandomLogoCssClass
{
    public static readonly IList<string> CssClasses = new List<string> { "Logo1", "Logo2", "Logo3", "Logo4", "Logo5", "Logo6", "Logo7", "Logo8", "Logo9", "Logo10", "Logo11" };

    public static string Run()
    {
        return Run(DateTime.Now);
    }

    public static string Run(DateTime dateTime)
    {
        var random = new Random(dateTime.DayOfYear);
        var index = random.Next(0, CssClasses.Count);

        return CssClasses[index];
    }
}
