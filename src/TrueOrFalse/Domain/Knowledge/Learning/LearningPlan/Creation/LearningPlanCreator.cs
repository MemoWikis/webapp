using System;
using System.Collections.Generic;

public class LearningPlanCreator
{
    public LearningPlan Run(Date date, LearningPlanSettings settings)
    {
        var learnPlan = new LearningPlan();

        learnPlan.Date = date;
        learnPlan.Questions = date.AllQuestions();

        return learnPlan;
    }
}

public class AnswerProbability
{
    public TimeSpan Offset;
    public int Probability;
    public Question Question;
}

public class NextDateFill
{
    public LearningDate Run(
        IList<Question> questions, 
        LearningPlanSettings settings)
    {
        var date = new LearningDate();

        //settings.IsInSnoozePeriod();

        //get offset
        //get all probablities for given time offset
        //order by answer probability desc 
     
           
        //forward 30minutes


        return date;
    }
}