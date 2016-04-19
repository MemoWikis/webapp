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
            .Each(s => LearningSessionStep.Skip(s.Id));

        IsCompleted = true;
    }
}
