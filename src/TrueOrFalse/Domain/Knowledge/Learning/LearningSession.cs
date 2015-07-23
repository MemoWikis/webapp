using System.Collections.Generic;
using Seedworks.Lib.Persistence;

public class LearningSession : DomainEntity, IRegisterAsInstancePerLifetime
{
    public virtual User User { get; set; }
    public virtual IList<LearningSessionStep> Steps{ get; set; }
    public virtual int LastCompletedStep { get; set; }
    public virtual Set SetToLearn { get; set; }

    public LearningSession()
    {
        
    }

}
