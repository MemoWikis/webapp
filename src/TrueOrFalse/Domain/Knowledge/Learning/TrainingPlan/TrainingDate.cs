﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using Seedworks.Lib.Persistence;

[DebuggerDisplay("{DateTime} AllQuestionsInTraining.Count:{AllQuestionsInTraining.Count}")]
public class TrainingDate : DomainEntity
{
    public virtual TrainingPlan TrainingPlan { get; set; }
    public virtual DateTime DateTime { get; set; }

    public virtual DateTime ExpiresAt { get; set; } = DateTime.MaxValue;
    public virtual string AllQuestionsJson
    {
        get { return JsonConvert.SerializeObject(AllQuestions); }
        set
        {
            if (value == null)
            {
                AllQuestions = new List<TrainingQuestion>();
                return;
            }

            AllQuestions = JsonConvert.DeserializeObject<IList<TrainingQuestion>>(value);
        }
    }

    public virtual IList<TrainingQuestion> AllQuestions { get; set; } = new List<TrainingQuestion>();

    public virtual IList<TrainingQuestion> AllQuestionsInTraining => AllQuestions.Where(x => x.IsInTraining).ToList();
    public virtual bool MarkedAsMissed { get; set; }
    public const int DateStaysOpenAfterNewBegunLearningStepInMinutes = 60;

    public virtual LearningSession LearningSession { get; set; }

    public virtual NotificationStatus NotificationStatus { get; set; } = NotificationStatus.None;

    public virtual bool IsBoostingDate { get; set; }

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

    public virtual bool IsExpired()
    {
        if (MarkedAsMissed)
            return true;

        return ExpiresAt <= DateTimeX.Now();
    }

    public virtual bool IsExpiredWithoutUpdate()
    {
        return IsExpired() && !MarkedAsMissed;
    }
}