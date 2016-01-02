using System;
using System.Collections.Generic;
using System.Linq;

public class TrainingPlanSettings
{
    public int QuestionsPerDate_Minimum = 7;
    public int QuestionsPerDate_IdealAmount = 10;

    public int SpacingBetweenSessionsInMinutes = 60*3;

    /// <summary>
    /// When answer probability drops below this treshold, 
    /// it should be trained soon. 
    /// </summary>
    public int AnswerProbabilityTreshhold = 90;

    public IList<Period> SnoozePeriods = new List<Period>{
        new Period(new Time(21, 00), new Time(9, 0))
    };

    public LearningPlanStrategy Strategy = LearningPlanStrategy.Sustained;

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