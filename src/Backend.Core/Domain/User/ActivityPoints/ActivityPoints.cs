[Serializable]
public class ActivityPoints : DomainEntity
{
    public virtual int Amount { get; set; }
    public virtual ActivityPointsType ActionType { get; set; }
    public virtual DateTime DateEarned { get; set; }
    public virtual int UserId { get; set; }
}