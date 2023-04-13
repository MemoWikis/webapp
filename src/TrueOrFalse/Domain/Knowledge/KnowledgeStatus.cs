﻿using System;
using System.Drawing;

public enum KnowledgeStatus
{
    NotLearned = 1,
    NeedsLearning = 2,
    NeedsConsolidation = 3,
    Solid = 4
}
// colors referenced from colors.less
public static class KnowledgeStatusColors
{
    public static string Solid = "#AFD534";
    public static string NeedsConsolidation = "#FDD648";
    public static string NeedsLearning = "#FFA07A";
    public static string NotLearned = "#DDDDDD";
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
            case KnowledgeStatus.Solid: return "#AFD534";
            default: Logg.r().Information("unknown Konwledge Status/ KonwledgeStatus.cs row 26 ");
                return "#C0C0C0";
        }
    }

    public static string GetText(this KnowledgeStatus status)
    {
        switch (status)
        {
            case KnowledgeStatus.NotLearned: return "Nicht gelernt";
            case KnowledgeStatus.NeedsLearning: return "Zu lernen";
            case KnowledgeStatus.NeedsConsolidation: return "Zu festigen";
            case KnowledgeStatus.Solid: return "Sicher gewusst";
            default:
                Logg.r().Information("unknown Knowledge Status/ KnowledgeStatus.cs row 40 ");
                return "Nicht gelernt";
        }
    }

    public static int GetProbability(this KnowledgeStatus status, int questionId)
    {
        switch (status)
        {
            case KnowledgeStatus.NotLearned:
                return ProbabilityCalc_Question.Run(
                    Sl.R<AnswerRepo>().GetByQuestion(questionId), useFirstAnswerPerUserOnly: true
                );
            case KnowledgeStatus.NeedsLearning: return 50;
            case KnowledgeStatus.NeedsConsolidation: return 80;
            case KnowledgeStatus.Solid: return 99;
            default:
                Logg.r().Information("unknown Knowledge Status/ KnowledgeStatus.cs row 57 ");
                return 30;
        }


    }

}