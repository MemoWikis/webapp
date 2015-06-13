using System;

public class UserActivity
{
    public virtual User User { get; set; }

    public string Info { get; set; }

    public DateTime At { get; set; }

    public virtual UserActivityType Type { get; set; }

    public virtual int TypeId { get; set; }
}