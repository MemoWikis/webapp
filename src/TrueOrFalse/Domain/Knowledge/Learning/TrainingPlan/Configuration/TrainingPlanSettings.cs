using System;
using System.Collections.Generic;
using System.Linq;

public class TrainingPlanSettings
{
    public virtual int QuestionsPerDate_Minimum { get; set; } = 7;
    public virtual int QuestionsPerDate_IdealAmount { get; set; } = 10;

    public virtual int SpacingBetweenSessionsInMinutes { get; set; } = 60 * 3;

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

    public virtual List<AnswerProbability> DebugAnswerProbabilities { get; set; }

    public bool IsInSnoozePeriod(DateTime dateTime)
    {
        return SnoozePeriods.Any(p => p.IsInPeriod(dateTime));
    }
}

public enum LearningPlanStrategy
{
    Sustained,
    //Grenznutzen
    MarginalUtility
}