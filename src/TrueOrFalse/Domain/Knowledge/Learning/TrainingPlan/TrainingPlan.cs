using System;
using System.Collections.Generic;
using System.Linq;
using Seedworks.Lib.Persistence;

public class TrainingPlan : DomainEntity
{

    public virtual Date Date { get; set; }

    public virtual IList<TrainingDate> Dates { get; set; }

    public virtual TimeSpan TimeRemaining { get; set; }
    public virtual TimeSpan TimeSpent { get; set; }

    /// <summary>Questions to train</summary>
    public virtual IList<TrainingQuestion> Questions
    {
        get
        {
            return Date == null 
                ? new List<TrainingQuestion>() 
                : Dates.SelectMany(x => x.Questions).ToList();
        }
    }
}