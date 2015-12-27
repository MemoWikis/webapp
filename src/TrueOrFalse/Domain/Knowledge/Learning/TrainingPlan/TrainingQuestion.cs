using Seedworks.Lib.Persistence;

public class TrainingQuestion : DomainEntity
{
    public virtual Question Question { get; set; }
    
    /// <summary>Probability Before</summary>
    public virtual int ProbBefore { get; set; }
    /// <summary>Probability After</summary>
    public virtual int ProbAfter { get; set; }
}