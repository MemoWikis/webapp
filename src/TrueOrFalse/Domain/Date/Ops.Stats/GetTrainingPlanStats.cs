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

        var dayCounter = DateTime.Now.Date;

        foreach (var tdDay in tdByDay)
        {
            while (dayCounter < tdDay.First().DateTime.Date)
            {
                result.TrainingPlanStatsPerDay.Add(new TrainingPlanStatsPerDayResult(dayCounter));
                dayCounter = dayCounter.AddDays(1);
            }
            var resultPartial = new TrainingPlanStatsPerDayResult(tdDay.First().DateTime.Date);
            tdDay.ToList().ForEach(d => resultPartial.TrainingDates.Add(d));

            if (resultPartial.TrainingDates.Count() > result.MaxNumberOfTrainingSessionsPerDay)
                result.MaxNumberOfTrainingSessionsPerDay = resultPartial.TrainingDates.Count();
            result.TrainingPlanStatsPerDay.Add(resultPartial);
            dayCounter = dayCounter.AddDays(1);
        }
        while (dayCounter <= trainingPlan.Date.DateTime.Date)
        {
            result.TrainingPlanStatsPerDay.Add(new TrainingPlanStatsPerDayResult(dayCounter));
            dayCounter = dayCounter.AddDays(1);
        }

        return result;
    }

    public string TrainingPlanStatsResult2GoogleDataTable(TrainingPlanStatsResult tpStatsResult)
    {
        StringBuilder result = new StringBuilder();
        result.Append("[");
        foreach (var tdDay in tpStatsResult.TrainingPlanStatsPerDay) 
        {
            var curTrainingSession = 1;
            var orgTrainingDateCount = tdDay.TrainingDates.Count;
            while (tdDay.TrainingSessionsCount < tpStatsResult.MaxNumberOfTrainingSessionsPerDay) //fill up IList with 0 until MaxNumberOTSPD
                tdDay.TrainingDates.Add(new TrainingDate());
            result.Append("[\"").Append(tdDay.Date.ToString("yyyy-MM-dd")).Append("\"");
            tdDay.TrainingDates.ToList().ForEach(d =>
            {
                result.Append(", ")
                    .Append(d.AllQuestionsInTraining.Count)
                    .Append(", \"<p style='margin: 5px; text-align: center;'><b>" + d.AllQuestionsInTraining.Count + " Fragen</b><br/>"+
                            "<span style='white-space: nowrap'>Übungssitzung " + curTrainingSession + " von "+ orgTrainingDateCount + "</span><br/>"+
                            "<span style='white-space: nowrap'>am " + d.DateTime.ToString("dd.MM. 'um' HH:mm") + "</span></p>\"");
                curTrainingSession++;
            });
            result.Append(" ],");
        }
        result.Length--; //remove last trailing comma
        result.Append("]");

        return result.ToString(); //should look like this: "[[\"1.5.\", 12, \"tooltip for previous number\", 9, \"tooltip for previous number\"],[\"2.5.\", 3, \"tooltip for previous number\", 4, \"tooltip for previous number\"]]";
    }
}
