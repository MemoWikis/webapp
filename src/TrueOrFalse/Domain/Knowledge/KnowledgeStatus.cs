using System;

public enum KnowledgeStatus
{
    NotLearned = 1,
    NeedsLearning = 2,
    NeedsConsolidation = 3,
    Solid = 4
}

public static class KnowledgeStatusExt
{
    public static string GetColorBgCss(this KnowledgeStatus status)
    {
        return @"background-color:" + GetColor(status);
    }

    public static string GetColor(this KnowledgeStatus status)
    {
        switch (status)
        {
            case KnowledgeStatus.NotLearned: return "#C0C0C0"; 
            case KnowledgeStatus.NeedsLearning: return "#FFA07A";
            case KnowledgeStatus.NeedsConsolidation: return "#FDD648";
            case KnowledgeStatus.Solid: return "#90EE90";
        }

        throw new Exception("unknown status");
    }

    public static string GetText(this KnowledgeStatus status)
    {
        switch (status)
        {
            case KnowledgeStatus.NotLearned: return "Nicht gelernt";
            case KnowledgeStatus.NeedsLearning: return "Zu lernen";
            case KnowledgeStatus.NeedsConsolidation: return "Zu festigen";
            case KnowledgeStatus.Solid: return "Sicher gewust";
        }

        throw new Exception("unknown status");
    }

}