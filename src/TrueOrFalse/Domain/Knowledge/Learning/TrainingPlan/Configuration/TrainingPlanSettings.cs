using System;
using System.Collections.Generic;
using System.Linq;

public class TrainingPlanSettings
{
    public virtual int QuestionsPerDate_Minimum { get; set; } = 7;
    public virtual int QuestionsPerDate_IdealAmount { get; set; } = 10;

    //public virtual int SpacingBetweenSessionsInMinutes { get; set; } = 60 * 3; //obsolete

    // after EqualizedSpacingDelayerDays days, MinSpacingBetweenSessionsInMinutes is multiplied by EqualizedSpacingMaxMultiplier/2
    public virtual int MinSpacingBetweenSessionsInMinutes { get; set; } = 60 * 3;
    public virtual bool EqualizeSpacingBetweenSessions { get; set; } = true;
    public virtual int EqualizedSpacingMaxMultiplier { get; set; } = 90; // higher values lead to higher spacing
    public virtual int EqualizedSpacingDelayerDays { get; set; } = 180; // higher values lead spacing effect to be effective when distance till date is greater

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

    public virtual int NumberOfHoursLastTrainingShouldEndBeforeDate { get; set; } = 3;

    public virtual List<AnswerProbability> DebugAnswerProbabilities { get; set; }

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

        /* spacing equalization is done via an arctan-function: [(2f/pi) * arctan(x/d)] * minSpacing.
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

}

public enum LearningPlanStrategy
{
    Sustained,
    //Grenznutzen
    MarginalUtility
}