﻿using System;
using System.Collections.Generic;
using System.Linq;

public class QuestionsCreatedPerDayResult
{
    public DateTime DateTime;

    public int CountByMemucho;
    public int CountByOthers;
    public int TotalCount { get { return CountByMemucho + CountByOthers; } }

    public static List<QuestionsCreatedPerDayResult> FillUpListWithZeros(List<QuestionsCreatedPerDayResult> list, DateTime from, DateTime to)
    {
        if (from.Date > to.Date)
            return list;
        var curDay = from.Date;
        while (curDay <= to.Date)
        {
            if (list.Find(i => i.DateTime == curDay) == null)
                list.Add(new QuestionsCreatedPerDayResult
                {
                    DateTime = curDay,
                    CountByOthers = 0,
                    CountByMemucho = 0
                });
            curDay = curDay.AddDays(1);
        }
        return list.OrderBy(i => i.DateTime).ToList();
    }

}
