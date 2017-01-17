using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UsageStatisticsOfLoggedInUsers
{
    public static IList<UsageStatisticsOfLoggedInUsersResult> InitListWithDates(DateTime since)
    {
        if (since.Date > DateTime.Now.Date)
            return null;
        var result = new List<UsageStatisticsOfLoggedInUsersResult>();
        var day = since.Date;
        while (day <= DateTime.Now.Date)
        {
            result.Add(new UsageStatisticsOfLoggedInUsersResult {DateTime = day});
            day = day.AddDays(1);
        }
        return result;
    }
}
