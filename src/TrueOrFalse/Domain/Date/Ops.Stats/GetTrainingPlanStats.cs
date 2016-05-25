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

        //todo: fill up empty dates with zeros, from now till date.

        foreach (var tdDay in tdByDay)
        {
            var resultPartial = new TrainingPlanStatsPerDayResult();
            resultPartial.Date = tdDay.First().DateTime.Date;
            //tpDay.ToList().ForEach(d => resultPartial.QuestionsInEachSession.Add(d.AllQuestions.Count()));
            foreach (var tdDaySession in tdDay)
                resultPartial.QuestionsInEachSession.Add(tdDaySession.AllQuestionsInTraining.Count());

            if (resultPartial.QuestionsInEachSession.Count() > result.MaxNumberOfTrainingSessionsPerDay)
                result.MaxNumberOfTrainingSessionsPerDay = resultPartial.QuestionsInEachSession.Count();
            result.TrainingPlanStatsPerDay.Add(resultPartial);
        }

        return result;
    }

    public string TrainingPlanStatsResult2Json(TrainingPlanStatsResult tpStatsResult)
    {
        StringBuilder result = new StringBuilder();
        result.Append("[");
        foreach (var tdDay in tpStatsResult.TrainingPlanStatsPerDay) //fill up IList with 0 until MaxNumberOTSPD
        {
            while (tdDay.TrainingSessionsCount < tpStatsResult.MaxNumberOfTrainingSessionsPerDay)
                tdDay.QuestionsInEachSession.Add(0);
            result.Append("[\"").Append(tdDay.Date).Append("\", ");
            tdDay.QuestionsInEachSession.Select(n => result.Append(n).Append(", ")); //needs to be "foreach"?
            result.Append(" \"\"]");
        }
        result.Append("]");

        //return "[[\"01.05.\", 12, 9, \"\"],[\"02.05.\", 3, 4, \"\"],[\"03.05.\", 6, 8, \"\"]]";
        return result.ToString();
    }
}
