using System;
using System.Collections.Generic;
using System.Linq;

public class TrainingPlanSettings
{
    public virtual int QuestionsPerDate_Minimum { get; set; }
    public virtual int QuestionsPerDate_IdealAmount { get; set; }

    public virtual int SpacingBetweenSessionsInMinutes { get; set; }

    /// <summary>
    /// When answer probability drops below this treshold, 
    /// it should be trained soon. 
    /// </summary>
    public virtual int AnswerProbabilityTreshhold { get; set; }

    public virtual IList<Period> SnoozePeriods { get; set; }

    public virtual LearningPlanStrategy Strategy { get; set; }

    public TrainingPlanSettings()
    {
        QuestionsPerDate_Minimum = 7;
        QuestionsPerDate_IdealAmount = 10;
        SpacingBetweenSessionsInMinutes = 60 * 3;
        AnswerProbabilityTreshhold = 90;
        SnoozePeriods = new List<Period>{
            new Period(new Time(21, 00), new Time(9, 0))
        };
        Strategy = LearningPlanStrategy.Sustained;
    }

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