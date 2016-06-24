using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Utils;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Web.Uris;

public class LearningSession : DomainEntity, IRegisterAsInstancePerLifetime
{
    public virtual User User { get; set; }
    public virtual IList<LearningSessionStep> Steps{ get; set; }

    public virtual Set SetToLearn { get; set; }
    public virtual Date DateToLearn { get; set; }

    public virtual bool IsCompleted { get; set; }

    public virtual string UrlName
    {
        get
        {
            if (SetToLearn != null)
                return UriSegmentFriendlyUser.Run(SetToLearn.Name);

            if (DateToLearn != null)
                return "Termin_" + DateToLearn.DateTime.ToString("D").Replace(",", "").Replace(" ", "_").Replace(".", "");

            throw new Exception("unknown session type");
        }
    }

    public virtual bool IsSetSession{ get { return SetToLearn != null; }}

    public virtual bool IsDateSession{ get { return DateToLearn != null; }}


    public virtual int TotalPossibleQuestions
    {
        get
        {
            if (IsSetSession)
                return SetToLearn.Questions().Count;

            if (IsDateSession)
                return DateToLearn.AllQuestions().Count;

            throw new Exception("unknown session type");
        }
    }

    public virtual int CurrentLearningStepIdx()
    {
        return Steps.ToList()
            .FindIndex(s => s.AnswerState == StepAnswerState.Uncompleted);
    }


    public virtual void CompleteSession()
    {
        if(IsCompleted) return;

        Steps.Where(s => s.AnswerState == StepAnswerState.Uncompleted)
            .Each(s => s.AnswerState = StepAnswerState.NotViewedOrAborted);

        IsCompleted = true;

        Sl.R<LearningSessionRepo>().Update(this);
    }

    public static LearningSession InitDateSession(Date date, TrainingDate trainingDate)
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
        }

        Sl.R<LearningSessionRepo>().Create(learningSession);

        if (trainingDate != null)
        {
            Sl.R<TrainingDateRepo>().Update(trainingDate);
        }

        return learningSession;
    }
}
