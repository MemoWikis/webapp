using System;
using Seedworks.Lib.Persistence;

/// <summary>
/// Relation between QuestionSet -> Question
/// Contains order
/// </summary>
[Serializable]
public class QuestionInSet : DomainEntity
{
    public virtual Set Set { get; set; }
    public virtual Question Question { get; set; }
    public virtual int Sort { get; set; }
}