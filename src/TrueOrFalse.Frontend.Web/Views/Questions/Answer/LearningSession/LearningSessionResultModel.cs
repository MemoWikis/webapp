using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class LearningSessionResultModel : BaseModel
{
    public LearningSession LearningSession;
    public int TotalNumberSteps;
    public int NumberCorrectAnswers;
    public int NumberWrongAnswers;
    public int NumberSkipped;
    public int NumberCorrectPercentage;
    public int NumberWrongAnswersPercentage;
    public int NumberSkippedPercentage;


    public LearningSessionResultModel(LearningSession learningSession)
    {
        LearningSession = learningSession;
        TotalNumberSteps = LearningSession.Steps.Count();

        if (TotalNumberSteps > 0)
        {
            var answeredSteps = LearningSession.Steps.Where(s => s.AnswerState == StepAnswerState.Answered).ToList();
            NumberCorrectAnswers = answeredSteps.Count(s => s.AnswerHistory.AnswerredCorrectly != AnswerCorrectness.False);
            NumberWrongAnswers = answeredSteps.Count(s => s.AnswerHistory.AnswerredCorrectly == AnswerCorrectness.False);

            NumberSkipped = LearningSession.Steps.Count(s => s.AnswerState == StepAnswerState.Skipped);

            NumberCorrectPercentage = (int)Math.Floor(NumberCorrectAnswers / (float)TotalNumberSteps * 100);
            NumberWrongAnswersPercentage = (int)Math.Floor(NumberWrongAnswers / (float)TotalNumberSteps * 100);
            NumberSkippedPercentage = (int)Math.Floor(NumberSkipped / (float)TotalNumberSteps * 100);
            //Loggen, wenn nicht Gesamtzahl ergeben
        }
    }
}
