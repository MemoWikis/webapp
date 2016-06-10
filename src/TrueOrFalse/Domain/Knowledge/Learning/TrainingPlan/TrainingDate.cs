using System;
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

        var upperTimeBound = ExpiresAt;

        if (LearningSession != null
            && !LearningSession.IsCompleted
            && LearningSession.DateCreated.AddHours(1) > ExpiresAt)
        {
            upperTimeBound = LearningSession.DateCreated.AddHours(1);
        }

        return upperTimeBound <= DateTimeX.Now();
    }

    public virtual bool IsExpiredWithoutUpdate()
    {
        return IsExpired() && !MarkedAsMissed;
    }

    public static LearningSession InitLearningSession(Date date, TrainingDate trainingDate)
    {
        var learningSession = new LearningSession
        {
            DateToLearn = date,
            User = date.User
        };

        if (trainingDate == null
            || (trainingDate.IsBoostingDate
                && !date.TrainingPlan.BoostingPhaseHasStarted()))
        {
            learningSession.Steps = GetLearningSessionSteps
                .Run(date.Sets.SelectMany(s => s.Questions()).ToList(),
                date.TrainingPlanSettings.QuestionsPerDate_Minimum);
        }
        else if (trainingDate.LearningSession != null)
        {
            learningSession = trainingDate.LearningSession;
        }
        else
        {
            learningSession.Steps = GetLearningSessionSteps.Run(trainingDate);
            trainingDate.LearningSession = learningSession;
            trainingDate.ExpiresAt = DateTime.Today.AddDays(1);
        }

        Sl.R<LearningSessionRepo>().Create(learningSession);

        if (trainingDate != null)
        {
            Sl.R<TrainingDateRepo>().Update(trainingDate);
        }

        return learningSession;
    }
}