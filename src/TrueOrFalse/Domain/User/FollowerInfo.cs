using Seedworks.Lib.Persistence;

public class FollowerInfo : DomainEntity
{
    public virtual User User { get; set; }
    public virtual User IFollow { get { return User; } }
    public virtual User Follower { get; set; }
}