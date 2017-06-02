using System;
using Seedworks.Lib.Persistence;

[Serializable]
public class ActivityReward : DomainEntity
{
    public virtual int Points { get; set; }
    public virtual ActivityPointsType ActionType { get; set; }
    public virtual DateTime DateEarned { get; set; }
    public virtual User User { get; set; }
}