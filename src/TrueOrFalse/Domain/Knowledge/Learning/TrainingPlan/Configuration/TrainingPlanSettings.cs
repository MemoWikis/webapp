﻿using System;
using System.Collections.Generic;
using System.Linq;

public class TrainingPlanSettings
{
    public virtual int QuestionsPerDate_Minimum { get; set; } = 7;
    public virtual int QuestionsPerDate_IdealAmount { get; set; } = 10;
    public const int QuestionsPerDate_IdealAmount_DefaultPercentageOfAllQuestions = 25;
    public const int QuestionsPerDate_IdealAmount_DefaultMinimum = 10;
    public const int QuestionsPerDate_IdealAmount_DefaultMaximum = 30;

    // after EqualizedSpacingDelayerDays days, MinSpacingBetweenSessionsInMinutes is multiplied by EqualizedSpacingMaxMultiplier/2
    public virtual int MinSpacingBetweenSessionsInMinutes { get; set; } = 60 * 10; //always use GetMinSpacingInMinutes() to acknowledge spacing effect
    public virtual bool EqualizeSpacingBetweenSessions { get; set; } = true;
    public virtual int EqualizedSpacingMaxMultiplier { get; set; } = 10; // higher values lead to higher spacing
    public virtual int EqualizedSpacingDelayerDays { get; set; } = 20; // higher values lead spacing effect to be effective when distance till date is greater

    public static int TryAddDateIntervalInMinutes = 15;

    /// <summary>
    /// When answer probability drops below this threshold, 
    /// it should be trained soon. 
    /// </summary>
    public virtual int AnswerProbabilityThreshold { get; set; } = 90;

    public virtual IList<Period> SnoozePeriods { get; set; } = new List<Period>{
        new Period(new Time(21, 00), new Time(9, 0))
    };

    public virtual LearningPlanStrategy Strategy { get; set; } = LearningPlanStrategy.Sustained;

    public virtual bool AddFinalBoost { get; set; } = true;

    public virtual List<AnswerProbability> AnswerProbabilities { get; set; }

    public virtual bool DebugLog { get; set; } = false;

    public bool IsInSnoozePeriod(DateTime dateTime)
    {
        return SnoozePeriods.Any(p => p.IsInPeriod(dateTime));
    }

    /// <summary>
    /// Returns the equalized (or straigthened out) minimal spacing between two learning sessions,
    /// according to the settings. If EqualizeSpacingBetweenSessions==true, then spacing increases 
    /// with distance to date, otherwise MinSpacingBetweenSessionsInMinutes is returned.
    /// </summary>
    public virtual int GetMinSpacingInMinutes(int distanceTillDateInDays)
    {
        if (!EqualizeSpacingBetweenSessions || (distanceTillDateInDays < 1))
            return MinSpacingBetweenSessionsInMinutes;

        /* spacing equalization is done via an arctan-function: [(f/(pi/2)) * arctan(x/d)] * minSpacing.
        ** x is number of days untill date (=exam)
        ** f distorts curve up, e.g.: higher f lead to higher spacing. (math: In infinity, minSpacing is factored by f)
        ** (division through pi/2 is done to normalize curve)
        ** d distorts curve to the right, e.g.: higher values lead spacing effect to be effective when distance till date is greater. After d days, minSpacing is factored by f/2
        ** min-value should be 1, e.g., spacing should be at least as great as MinSpacing in Settings
        */
        return
            Convert.ToInt32(
                Math.Max(1,
                    (EqualizedSpacingMaxMultiplier / (Math.PI / 2)) *
                    Math.Atan((double)distanceTillDateInDays / EqualizedSpacingDelayerDays)) *
                MinSpacingBetweenSessionsInMinutes);
        
    }

    public static TrainingPlanSettings GetSettingsWithIndividualDefaults(Date date)
    {
        var questionsPerDate_IdealAmount = 
            (int) Math.Round(
                date.CountQuestions() * QuestionsPerDate_IdealAmount_DefaultPercentageOfAllQuestions / (double)100,
                MidpointRounding.AwayFromZero);
        questionsPerDate_IdealAmount = Math.Max(QuestionsPerDate_IdealAmount_DefaultMinimum, questionsPerDate_IdealAmount);
        questionsPerDate_IdealAmount = Math.Min(questionsPerDate_IdealAmount, QuestionsPerDate_IdealAmount_DefaultMaximum);

        return new TrainingPlanSettings { QuestionsPerDate_IdealAmount = questionsPerDate_IdealAmount };
    }

}

public enum LearningPlanStrategy
{
    Sustained,
    //Grenznutzen
    MarginalUtility
}