using System;
using System.Collections.Generic;
using Seedworks.Lib.Persistence;

public class TrainingDate : DomainEntity
{
    public virtual DateTime DateTime { get; set; }
    public virtual IList<Question> Questions { get; set; }
}