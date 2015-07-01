using System.Collections.Generic;
using Seedworks.Lib.Persistence;

public class LearningSession : DomainEntity, IRegisterAsInstancePerLifetime
{
    public virtual IList<LearningSessionStep> Steps{ get; set; }

    public LearningSession()
    {
        
    }

}
