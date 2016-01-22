using System;
using System.Collections.Generic;
using System.Linq;
using Seedworks.Lib.Persistence;

public class TrainingDate : DomainEntity
{
    public virtual DateTime DateTime { get; set; }

    public virtual IList<TrainingQuestion> AllQuestions { get; set; }
    public virtual IList<TrainingQuestion> AllQuestionsInTraining
    {
        get { return AllQuestions.Where(x => x.IsInTraining).ToList(); }
    }

    public TrainingDate()
    {
        AllQuestions = new List<TrainingQuestion>();
    }
}