using System.Diagnostics;


[Serializable]
[DebuggerDisplay("User:{User.Name} Follower:{Follower.Name}")]
public class FollowerInfo : DomainEntity
{
    public virtual User User { get; set; }
    public virtual User Follower { get; set; }
}