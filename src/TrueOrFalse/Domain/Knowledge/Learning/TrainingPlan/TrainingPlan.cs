using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Util;
using Seedworks.Lib.Persistence;

public class TrainingPlan : DomainEntity
{
    public virtual Date Date { get; set; }

    public virtual IList<TrainingDate> Dates { get; set; } = new List<TrainingDate>();
    public virtual IList<TrainingDate> OpenDates => Dates
                                                        .Where(d => d.DateTime > DateTimeX.Now()
                                                            && !d.MarkedAsMissed
                                                            && d.LearningSession == null)
                                                        .OrderBy(d => d.DateTime).ToList();
    public virtual IList<TrainingDate> PastDates => Dates.Except(OpenDates).OrderBy(d => d.DateTime).ToList();

    public virtual TimeSpan TimeToNextDate => HasOpenDates ? GetNextTrainingDate().DateTime - DateTime.Now : new TimeSpan(0, 0, 0);

    public const int SecondsPerQuestionEst = 30;

    public virtual TimeSpan TimeRemaining => new TimeSpan(0, 0, seconds: (OpenDates.Count * Questions.Count) * SecondsPerQuestionEst);
    public virtual TimeSpan TimeSpent => new TimeSpan(0, 0, seconds: (PastDates.Count * Questions.Count) * SecondsPerQuestionEst);

    public virtual bool HasOpenDates => OpenDates.Any();
    public virtual TrainingPlanSettings Settings { get; set; }

    /// <summary>Questions to train</summary>
    public virtual IList<Question> Questions 
        => Date == null 
            ? new List<Question>()
            : Date.AllQuestions();

    public virtual bool HasNewMissedDates()
    {
        return PastDates.Any(d => d.LearningSession == null && !d.MarkedAsMissed);
    }

    public virtual void MarkDatesAsMissed()
    {
        PastDates.ForEach(d =>
        {
            if (d.LearningSession != null || d.MarkedAsMissed) return;
            d.MarkedAsMissed = true;
            Sl.Resolve<TrainingDateRepo>().Update(d);
        }
       );
    }

    public virtual TrainingDate GetNextTrainingDate()
    {
        PastDates.Where(d => d.LearningSession != null && !d.LearningSession.IsCompleted).ForEach(d => d.LearningSession.CompleteSession());
        return HasOpenDates ? OpenDates.First() : null;
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