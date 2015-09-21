using System;
using System.Collections.Generic;

public class BadgeType
{
    public virtual string Key { get; set; }
    public virtual string Name { get; set; }
    public virtual string Description { get; set; }

    public virtual bool IsSecret { get; set; }

    public virtual IList<BadgeLevel> Levels { get; set; }
    public virtual BadgeTypeGroup Group { get; set; }

    public virtual BadgeCheckOn[] BadgeCheckOn  { get; set; }
    public virtual Func<BadgeTypeFilterParams, bool> DoesApply { get; set; }

    public BadgeType()
    {
        Levels = new List<BadgeLevel>();
    }
}