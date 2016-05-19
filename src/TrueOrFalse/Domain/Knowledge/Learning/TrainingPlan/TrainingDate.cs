using System;
using System.Collections.Generic;
using System.Linq;
using Seedworks.Lib.Persistence;

public class TrainingDate : DomainEntity
{
    public virtual TrainingPlan TrainingPlan { get; set; }
    public virtual DateTime DateTime { get; set; }
    public virtual IList<TrainingQuestion> AllQuestions { get; set; } = new List<TrainingQuestion>();
    public virtual IList<TrainingQuestion> AllQuestionsInTraining => AllQuestions.Where(x => x.IsInTraining).ToList();
    public virtual bool MarkedAsMissed { get; set; }

    public virtual LearningSession LearningSession { get; set; }

    public virtual NotificationStatus NotificationStatus { get; set; } = NotificationStatus.None;

    public virtual User User() { return TrainingPlan.Date.User; }
    public virtual string UserEmail(){ return User().EmailAddress; }

    public virtual TimeSpan TimeEstimated() { return new TimeSpan(0, 0, seconds: AllQuestionsInTraining.Sum(q => q.Question.TimeToLearnInSeconds())); }

    public virtual KnowledgeSummary GetSummaryBefore()
    {
        return KnowledgeSummaryLoader.Run(AllQuestions, beforeTraining:true);
    }

    public virtual KnowledgeSummary GetSummaryAfter()
    {
        return KnowledgeSummaryLoader.Run(AllQuestions, afterTraining: true);
    }
}