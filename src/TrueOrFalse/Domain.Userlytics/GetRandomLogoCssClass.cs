using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GetRandomLogoCssClass
{
    public static readonly IList<string> CssClasses = new List<string>{"Logo1", "Logo2", "Logo3"};

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
