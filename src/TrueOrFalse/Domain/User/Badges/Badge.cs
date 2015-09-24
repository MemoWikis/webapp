using System.Diagnostics;
using Seedworks.Lib.Persistence;

[DebuggerDisplay("{BadgeTypeKey} {Level}")]
public class Badge : DomainEntity
{
    public virtual string BadgeTypeKey { get; set; }

    public virtual int TimesGiven { get; set; }

    public virtual int Points { get; set; }
    public virtual string Level { get; set; }

    public virtual User User { get; set; }
}