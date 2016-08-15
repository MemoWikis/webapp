using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;

public class TrainingPlanStatsPerDayResult
{
    public DateTime Date;

    public int TrainingSessionsCount { get { return TrainingDates.Count; } }
    public IList<TrainingDate> TrainingDates = new List<TrainingDate>();

    public TrainingPlanStatsPerDayResult(DateTime dateTime)
    {
        Date = dateTime;
    }
}