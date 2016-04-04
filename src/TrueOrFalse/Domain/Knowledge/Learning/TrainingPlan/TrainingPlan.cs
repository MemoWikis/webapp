using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Persistence;

public class TrainingPlan : DomainEntity
{
    public virtual Date Date { get; set; }

    public virtual IList<TrainingDate> Dates { get; set; } = new List<TrainingDate>();
    public virtual IList<TrainingDate> DatesInFuture => Dates.Where(d => d.DateTime > DateTimeX.Now()).OrderBy(d => d.DateTime).ToList();
    public virtual IList<TrainingDate> DatesInPast => Dates.Where(d => d.DateTime <= DateTimeX.Now()).OrderBy(d => d.DateTime).ToList();

    public virtual TimeSpan TimeToNextDate => HasDatesInFuture ? GetNextTrainingDate().DateTime - DateTime.Now : new TimeSpan(0, 0, 0);

    public const int SecondsPerQuestionEst = 30;

    public virtual TimeSpan TimeRemaining => new TimeSpan(0, 0, seconds: (DatesInFuture.Count * Questions.Count) * SecondsPerQuestionEst);
    public virtual TimeSpan TimeSpent => new TimeSpan(0, 0, seconds: (DatesInPast.Count * Questions.Count) * SecondsPerQuestionEst);

    public virtual bool HasDatesInFuture => DatesInFuture.Any();
    public virtual TrainingPlanSettings Settings { get; set; }

    /// <summary>Questions to train</summary>
    public virtual IList<Question> Questions 
        => Date == null 
            ? new List<Question>()
            : Date.AllQuestions();

    public virtual TrainingDate GetNextTrainingDate()
    {
        return HasDatesInFuture ? DatesInFuture.First() : null;
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