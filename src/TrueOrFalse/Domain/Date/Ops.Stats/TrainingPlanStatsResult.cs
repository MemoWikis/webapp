using System;
using System.Collections.Generic;

public class TrainingPlanStatsResult
{
    public IList<TrainingPlanStatsPerDayResult> TrainingPlanStatsPerDay = new List<TrainingPlanStatsPerDayResult>();
    public int MaxNumberOfTrainingSessionsPerDay;
}