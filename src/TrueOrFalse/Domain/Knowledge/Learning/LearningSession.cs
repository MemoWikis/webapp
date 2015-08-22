using System.Collections.Generic;
using System.Linq;
using Seedworks.Lib.Persistence;

public class LearningSession : DomainEntity, IRegisterAsInstancePerLifetime
{
    public virtual User User { get; set; }
    public virtual IList<LearningSessionStep> Steps{ get; set; }
    public virtual Set SetToLearn { get; set; }

    public LearningSession()
    {
        
    }

    public virtual int CurrentLearningStepIdx()
    {
        return Steps.ToList()
            .FindIndex(s => s.AnswerState == StepAnswerState.Uncompleted);
    }
}
