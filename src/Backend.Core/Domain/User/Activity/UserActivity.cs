﻿public class UserActivity : DomainEntity
{
    public virtual User UserConcerned { get; set; }
    public virtual DateTime At { get; set; }
    public virtual UserActivityType Type { get; set; }
    public virtual Question Question { get; set; }
    public virtual Page Page { get; set; }

    /// <summary>
    /// relevant for activity "FollowedUser": UserCauser follows UserIsFollowed
    /// </summary>
    public virtual User UserIsFollowed { get; set; }

    /// <summary>
    /// this is the one that triggered the activity
    /// </summary>
    public virtual User UserCauser { get; set; }
}