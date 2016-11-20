using System;
using System.Collections.Generic;
using System.Linq;

public class GetSampleDates
{
    public static IList<Date> Run()
    {
        var sets = Sl.R<SetRepo>()
            .GetByIds(44, 45, 19, 30, 35, 49, 42);

        var now = DateTime.Now;

        return new List<Date>
        {
            new Date
            {
                Details = "Test Europäische Union",
                Sets = sets.Where(s => s.Id == 44 || s.Id == 45 || s.Id == 19).ToList(),
                DateTime = new DateTime(now.Year, now.Month, now.Day, 10, 00, 0).AddDays(4)
            },
            new Date
            {
                Details = "LEK Literaturepochen",
                Sets = sets.Where(s => s.Id == 30 || s.Id == 35).ToList(),
                DateTime = new DateTime(now.Year, now.Month, now.Day, 15, 30, 0).AddDays(9)
            },
            new Date
            {
                Details = "Mdl Prüfung Geschichte",
                Sets = sets.Where(s => s.Id == 49 || s.Id == 42).ToList(),
                DateTime = new DateTime(now.Year, now.Month, now.Day, 8, 00, 0).AddDays(20)
            }
        };
    }

    
    /// <summary>
    /// Does the same as Run(), but returns different dates.
    /// To be used for example when dates in network are needed
    /// </summary>
    public static IList<Date> RunAgain()
    {
        var sets = Sl.R<SetRepo>()
            .GetByIds(27, 14, 17);

        var now = DateTime.Now;

        return new List<Date>
        {
            new Date
            {
                Details = "Einbürgerungstest",
                Sets = sets.Where(s => s.Id == 27).ToList(),
                DateTime = new DateTime(now.Year, now.Month, now.Day, 15, 00, 0).AddDays(5)
            },
            new Date
            {
                Details = "Kneipenquiz",
                Sets = sets.Where(s => s.Id == 14 || s.Id == 17).ToList(),
                DateTime = new DateTime(now.Year, now.Month, now.Day, 15, 30, 0).AddDays(13)
            }
        };
    }
}