public class TrainingQuestion
{
    public Question Question { get; set; }
    
    /// <summary>Probability Before</summary>
    public int ProbBefore { get; set; }
    /// <summary>Probability After</summary>
    public int ProbAfter { get; set; }

    public bool IsInTraining { get; set; }
}