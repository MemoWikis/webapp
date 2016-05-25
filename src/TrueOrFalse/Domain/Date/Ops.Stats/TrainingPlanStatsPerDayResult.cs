using System;
using System.Collections.Generic;
using System.Linq;

public class TrainingPlanStatsPerDayResult
{
    public DateTime Date;

    public int TrainingSessionsCount { get { return QuestionsInEachSession.Count; } }
    public IList<int> QuestionsInEachSession = new List<int>();
    public int TotalQuestionsPerDay { get { return QuestionsInEachSession.Sum(); } }
}