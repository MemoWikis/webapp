using System;
using System.Collections.Generic;

public class BadgeType
{
    public virtual string Key { get; set; }
    public virtual string Name { get; set; }
    public virtual string Description { get; set; }

    public virtual bool IsSecret { get; set; }

    /// <summary>Can be assigned multiple times</summary>
    public virtual bool MoreThanOnce { get; set; }

    public virtual IList<BadgeLevel> Levels { get; set; }
    public virtual BadgeTypeGroup Group { get; set; }

    public virtual BadgeCheckOn[] BadgeCheckOn  { get; set; }
    public virtual Func<BadgeAwardCheckParams, BadgeAwardCheckResult> Awarded { get; set; }

    public BadgeType()
    {
        Levels = new List<BadgeLevel>();
    }
}