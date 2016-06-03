using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Seedworks.Lib.Persistence;

[DebuggerDisplay("QuestionId={Question.Id} IsInTraining={IsInTraining}")]
public class TrainingQuestion : DomainEntity
{
    public virtual Question Question { get; set; }
    
    /// <summary>Probability Before</summary>
    public virtual int ProbBefore { get; set; }
    /// <summary>Probability After</summary>
    public virtual int ProbAfter { get; set; }

    public virtual bool IsInTraining { get; set; }
}