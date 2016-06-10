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
  
    public virtual IList<TrainingDate> OpenDates =>
        Dates
            .Where(d => !d.IsExpired()
                && !d.MarkedAsMissed
                && (d.LearningSession == null || !d.LearningSession.IsCompleted))
            .OrderBy(d => d.DateTime).ToList();

    public virtual IList<TrainingDate> PastDates => Dates.Except(OpenDates).OrderBy(d => d.DateTime).ToList();

    public virtual IList<TrainingDate> PastDatesNotMissed =>
        PastDates
            .Where(d => !d.MarkedAsMissed
                && d.LearningSession != null)
            .OrderBy(d => d.DateTime).ToList();

    public virtual TimeSpan TimeToNextDate => HasOpenDates ? GetNextTrainingDate().DateTime - DateTime.Now : new TimeSpan(0, 0, 0);
    public virtual int QuestionCountInNextDate => HasOpenDates ? GetNextTrainingDate().AllQuestionsInTraining.Count() : 0;

    public const int SecondsPerQuestionEst = 20;

    //public virtual TimeSpan TimeRemaining => new TimeSpan(0, 0, seconds: (OpenDates.Count * Questions.Count) * SecondsPerQuestionEst);
    public virtual TimeSpan TimeRemaining => new TimeSpan(0, 0, seconds: OpenDates.SelectMany(d => d.AllQuestionsInTraining).Sum(q => q.Question.TimeToLearnInSeconds()));
    //public virtual TimeSpan TimeSpent => new TimeSpan(0, 0, seconds: (PastDatesNotMissed.Count * Questions.Count) * SecondsPerQuestionEst);

    public virtual bool HasOpenDates => OpenDates.Any();
    public virtual TrainingPlanSettings Settings { get; set; }

    public virtual bool LearningGoalIsReached { get; set; } = false;

    public virtual string UserMessage { get; set; }

    /// <summary>Questions to train</summary>
    public virtual IList<Question> Questions 
        => Date == null 
            ? new List<Question>()
            : Date.AllQuestions();

    public virtual void MarkDatesAsMissed()
    {
        var trainingDateRepo = Sl.Resolve<TrainingDateRepo>();

        PastDates.ForEach(d => 
        {
            if (!d.IsExpired() || d.LearningSession != null || d.MarkedAsMissed) return;

            d.MarkedAsMissed = true;
            trainingDateRepo.Update(d);
        });

        trainingDateRepo.Flush();
    }

    public virtual List<Question> BoostedQuestions()
    {
        return Dates
            .Where(d => d.LearningSession != null && d.IsBoostingDate)
            .SelectMany(d => d.LearningSession.Steps
                .Where(s => s.AnswerState == StepAnswerState.Answered)
                .Select(s => s.Question))
                .ToList();
    }

    public virtual bool BoostingPhaseHasStarted()
    {
        if (!Settings.AddFinalBoost || !Dates.Any(d => d.IsBoostingDate))
            return false;

        return Dates.First(d => d.IsBoostingDate).DateTime <= DateTimeX.Now();
    }

    public virtual TrainingDate GetNextTrainingDate(bool withUpdate = false)
    {

        if (withUpdate && PastDates.Any(d => d.IsExpiredWithoutUpdate()))
        {
            TrainingPlanUpdater.Run(this, Settings);
        }

        return OpenDates.FirstOrDefault();
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

    public virtual void CompleteUnfinishedSessions()
    {
        PastDates.Where(d => d.LearningSession != null 
                        && !d.LearningSession.IsCompleted 
                        && d.IsExpired())
                .ForEach(d => d.LearningSession.CompleteSession());
    }
}