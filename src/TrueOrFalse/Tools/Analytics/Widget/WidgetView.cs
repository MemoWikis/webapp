using System;
using Seedworks.Lib.Persistence;


public class WidgetView : Entity, WithDateCreated
{
    public virtual string Host { get; set; }

    public virtual string WidgetKey { get; set; }

    public virtual WidgetType WidgetType { get; set; }

    public virtual int EntityId { get; set; }

    public virtual DateTime DateCreated { get; set; }


    public static string GetDescriptionForWidgetType(WidgetType widgetType)
    {
        switch (widgetType)
        {
                case WidgetType.Question: return "Frage in Einzelfrage-Widget";
                case WidgetType.SetStartPage: return "Lernset Startseite";
                case WidgetType.SetStepPage: return "Einzelfrage in Lernset";
                case WidgetType.SetResult: return "Ergebnisseite Lernset";
                case WidgetType.SetVideo: return "Video-Lernset";
            default: return "";
        }
    }
}

public enum WidgetType
{
    Question = 0,
    SetStartPage = 1,
    SetStepPage = 2,
    SetResult = 3,
    SetVideo = 4
}