using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Persistence;

public class TrainingPlan : DomainEntity
{
    public virtual Date Date { get; set; }

    public virtual IList<TrainingDate> Dates { get; set; }

    public virtual TimeSpan TimeRemaining { get; set; }
    public virtual TimeSpan TimeSpent { get; set; }

    public virtual TrainingPlanSettings Settings { get; set; }

    /// <summary>Questions to train</summary>
    public virtual IList<TrainingQuestion> Questions
    {
        get
        {
            return Date == null 
                ? new List<TrainingQuestion>() 
                : Dates.SelectMany(x => x.AllQuestions).ToList();
        }
    }

    public virtual void DumpToConsole()
    {
        var sb = new StringBuilder();
        sb.AppendLine("DatesCount:" + Dates.Count);

        foreach (var date in Dates)
        {
            sb.Append(date.DateTime.ToString("g"));
            sb.Append("  q:" + date.AllQuestionsInTraining.Count +  " ");
            sb.Append(date.AllQuestions.Select(q => q.ProbBefore + "/" + q.ProbAfter).Aggregate((a, b) => a + " " + b));
            
            sb.AppendLine("");
        }

        Console.Write(sb);
    }
}