using System;
using System.Collections.Generic;
using System.Linq;

public class TrainingPlanSettings
{
    public virtual int QuestionsPerDate_Minimum { get; set; } = 7;
    public virtual int QuestionsPerDate_IdealAmount { get; set; } = 10;

    public virtual int SpacingBetweenSessionsInMinutes { get; set; } = 60 * 3; //will be obsolete, to delete in mapping as well

    // aprox.: after EqualizedSpacingBetweenSessionsDividerDays days, MinSpacingBetweenSessionsInMinutes is multiplied by EqualizedSpacingBetweenSessionsFactorMax
    public virtual int MinSpacingBetweenSessionsInMinutes { get; set; } = 60 * 3;
    public virtual int EqualizedSpacingBetweenSessionsFactorMax { get; set; } = 60; // higher values lead to higher spacing
    public virtual int EqualizedSpacingBetweenSessionsDividerDays { get; set; } = 180; // higher values lead spacing effect to be effective when distance till date is greater

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

    public virtual int GetEqualizedMinSpacingInMinutes(int distanceTillDateInDays, int questionCount = 100)
    {
        //todo: does not account for number of questions yet

        //double distanceTillDateInDays = (Date.DateTime - proposedDateTime).Days; //needs to be double to not lose fraction in division below

        /* spacing is done via an arctan-function: f * arctan(x/d).
        ** x is number of days untill date (=exam)
        ** f distorts curve up, e.g.: higher f lead to higher spacing
        ** d distorts curve to the right, e.g.: higher values lead spacing effect to be effective when distance till date is greater
        ** min-value should be 1, e.g., spacing should be at least as great as MinSpacing in Settings
        */
        if (distanceTillDateInDays > 0)
            return
                Convert.ToInt32(
                    Math.Max(1,
                        EqualizedSpacingBetweenSessionsFactorMax *
                        Math.Atan((double)distanceTillDateInDays / EqualizedSpacingBetweenSessionsDividerDays)) *
                    MinSpacingBetweenSessionsInMinutes);
        else
            return MinSpacingBetweenSessionsInMinutes;
    }

}

public enum LearningPlanStrategy
{
    Sustained,
    //Grenznutzen
    MarginalUtility
}