using System;
using System.Collections.Generic;

public class LearningPlanCreator
{
    public LearningPlan Run(Date date, LearningPlanSettings settings)
    {
        var learnPlan = new LearningPlan();

        learnPlan.Date = date;
        learnPlan.Questions = date.AllQuestions();
        learnPlan.LearningDates = GetDates(date, settings);

        return learnPlan;
    }

    private IList<LearningDate> GetDates(Date date, LearningPlanSettings settings)
    {
        var nextDate = DateTime.Now.AddMinutes(10);


        

        //settings.IsInSnoozePeriod();

        //get offset
        //get all probablities for given time offset
        //order by answer probability desc 


        //forward 30minutes


        return null;
    }

    private LearningDate GetNextDate()
    {
        return null;
    }
}

public class AnswerProbability
{
    public TimeSpan Offset;
    public int Probability;
    public Question Question;
}