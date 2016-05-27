using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using System.Text;

public class GetTrainingPlanStats : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public GetTrainingPlanStats(ISession session)
    {
        _session = session; //todo: do I need this here at all?
    }

    public TrainingPlanStatsResult Run(TrainingPlan trainingPlan)
    {
        var tdByDay = trainingPlan.OpenDates.OrderBy(d => d.DateTime).GroupBy(t => t.DateTime.Date);
        var result = new TrainingPlanStatsResult();

        var prevDay = DateTime.Now.Date;

        foreach (var tdDay in tdByDay)
        {
            prevDay = prevDay.AddDays(1);
            while (prevDay < tdDay.First().DateTime.Date)
            {
                result.TrainingPlanStatsPerDay.Add(new TrainingPlanStatsPerDayResult(prevDay));
                prevDay = prevDay.AddDays(1);
            }
            var resultPartial = new TrainingPlanStatsPerDayResult(tdDay.First().DateTime.Date);
            tdDay.ToList().ForEach(d => resultPartial.QuestionsInEachSession.Add(d.AllQuestionsInTraining.Count()));

            if (resultPartial.QuestionsInEachSession.Count() > result.MaxNumberOfTrainingSessionsPerDay)
                result.MaxNumberOfTrainingSessionsPerDay = resultPartial.QuestionsInEachSession.Count();
            result.TrainingPlanStatsPerDay.Add(resultPartial);
        }
        while (prevDay < trainingPlan.Date.DateTime.Date)
        {
            prevDay = prevDay.AddDays(1);
            result.TrainingPlanStatsPerDay.Add(new TrainingPlanStatsPerDayResult(prevDay));
        }

        return result;
    }

    public string TrainingPlanStatsResult2GoogleDataTable(TrainingPlanStatsResult tpStatsResult)
    {
        StringBuilder result = new StringBuilder();
        result.Append("[");
        foreach (var tdDay in tpStatsResult.TrainingPlanStatsPerDay) //fill up IList with 0 until MaxNumberOTSPD
        {
            while (tdDay.TrainingSessionsCount < tpStatsResult.MaxNumberOfTrainingSessionsPerDay)
                tdDay.QuestionsInEachSession.Add(0);
            result.Append("[\"").Append(tdDay.Date.ToString("d. M.")).Append("\"");
            tdDay.QuestionsInEachSession.ToList().ForEach(n => result.Append(", ").Append(n));
            result.Append(" ],");
        }
        result.Length--; //remove last trailing comma
        result.Append("]");

        return result.ToString(); //should look like this: "[[\"01.05.\", 12, 9],[\"02.05.\", 3, 4],[\"03.05.\", 6, 8]]";
    }
}
