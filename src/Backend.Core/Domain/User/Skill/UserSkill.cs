/// <summary>
/// Represents a user's skill level for a specific page or wiki, calculated based on their question answering performance
/// </summary>
public class UserSkill : DomainEntity
{
    public virtual int UserId { get; set; }
    public virtual int PageId { get; set; }
    
    /// <summary>
    /// JSON containing the calculated skill evaluation data using KnowledgeSummary structure
    /// This is automatically calculated based on the user's performance on questions within this page/wiki
    /// Example: { "NotLearned": 5, "NeedsLearning": 10, "NeedsConsolidation": 15, "Solid": 20, "Options": "standard" }
    /// </summary>
    public virtual string? EvaluationJson { get; set; }
    
    /// <summary>
    /// When this skill was first calculated/added to the user's profile
    /// </summary>
    public virtual DateTime AddedAt { get; set; }
    
    /// <summary>
    /// Last time the skill evaluation was recalculated based on new question performance
    /// </summary>
    public virtual DateTime? LastUpdatedAt { get; set; }
}
