using System;
using Seedworks.Lib.Persistence;

public class WidgetView : Entity, WithDateCreated
{
    public virtual string Host { get; set; }

    public virtual string WidgetKey { get; set; }

    public virtual DateTime DateCreated { get; set; }
}