using System;
using System.Collections.Generic;
using System.Linq;
using Seedworks.Lib;
using Seedworks.Lib.Persistence;

public class Date : DomainEntity
{
    public virtual string Details { get; set; }

    /// <summary>
    /// The date and time, when the date occurs
    /// </summary>
    public virtual DateTime DateTime { get; set; }

    public virtual TrainingPlan TrainingPlan { get; set; }
    public virtual string TrainingPlanJson { get; set; }
    public virtual IList<LearningSession> LearningSessions { get; set; }
    public virtual User User { get; set; }
    public virtual IList<Set> Sets { get; set; }

    public virtual DateVisibility Visibility { get; set; }

    public virtual TimeSpan TrainingTimeRemaining => TrainingPlan.TimeRemaining;
    public virtual bool HasOpenDates => TrainingPlan.HasOpenDates;
    public virtual TimeSpan TimeToNextDate => TrainingPlan.TimeToNextDate;

    public virtual TrainingPlanSettings TrainingPlanSettings => TrainingPlan.Settings;


    public virtual Date CopiedFrom { get; set; }
    public virtual IList<Date> CopiedInstances { get; set; }

    public Date()
    {
        Sets = new List<Set>();
        LearningSessions = new List<LearningSession>();
    }

    public virtual IList<Question> AllQuestions()
    {
        return Sets
            .SelectMany(s => s.QuestionsInSet.Select(qs => qs.Question))
            .Distinct()
            .ToList();
    }

    public virtual int CountQuestions() => AllQuestions().Count;
    public virtual int LearningSessionQuestionsAnswered() => 
        LearningSessions.SelectMany(s => s.Steps).Count(q => q.AnswerState == StepAnswerState.Answered);

    public virtual TimeSpan TimeSpentLearning()
    {
        return new TimeSpan(0, 0, LearningSessions.SelectMany(l => l.Steps.Select(s => s.Question)).Sum(q => q.TimeToLearnInSeconds()));
    }

    public virtual string GetTitle(bool shorten = false)
    {
        if (Details == null)
            Details = "";

        if (Details.Length > 6)
            Details.WordWrap(50);

        if (shorten && Details.Length > 40)
            return Details.Truncate(40) + "...";

        if (Details.Length > 0)
            return Details;

        return "Am " + DateTime.ToString("dd.MM.yyy") + " um " + DateTime.ToString("HH:mm");
    }

    public virtual TimeSpan Remaining() => DateTime - DateTime.Now;
    public virtual int RemainingDays() => Math.Abs(Convert.ToInt32(Remaining().TotalDays));
    public virtual int RemainingMinutes() => Math.Abs(Convert.ToInt32(Remaining().TotalMinutes));
    public virtual TimeSpanLabel RemainingLabel() => new TimeSpanLabel(Remaining());
}