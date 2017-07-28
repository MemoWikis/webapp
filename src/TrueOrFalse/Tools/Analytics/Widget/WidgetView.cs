using System;
using Seedworks.Lib.Persistence;


public class WidgetView : Entity, WithDateCreated
{
    public virtual string Host { get; set; }

    public virtual string WidgetKey { get; set; }

    public virtual WidgetType WidgetType { get; set; }

    public virtual int EntityId { get; set; }

    public virtual DateTime DateCreated { get; set; }
}

public enum WidgetType
{
    Question = 0,
    SetStartPage = 1,
    SetStepPage = 2,
    SetResult = 3,
    SetVideo = 4
}