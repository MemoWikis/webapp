using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class LearningSessionResultModel : BaseModel
{
    public LearningSession LearningSession;
    public int NumberSteps;
    public int NumberQuestions;
    public int NumberCorrectAnswers; //answered correctly at first try
    public int NumberCorrectAfterRepetitionAnswers; 
    public int NumberWrongAnswers;
    public int NumberNotAnswered;
    public int NumberCorrectPercentage;
    public int NumberCorrectAfterRepetitionPercentage;
    public int NumberWrongAnswersPercentage;
    public int NumberNotAnsweredPercentage;


    public LearningSessionResultModel(LearningSession learningSession)
    {
        LearningSession = learningSession;
        NumberSteps = LearningSession.Steps.Count();
        NumberQuestions = LearningSession.Questions().Count;

        if (NumberSteps > 0)
        {
            var answeredSteps = LearningSession.Steps.Where(s => s.AnswerState == StepAnswerState.Answered).ToList();
            NumberCorrectAnswers = answeredSteps.Count(s => s.Answer.AnswerredCorrectly != AnswerCorrectness.False);
            NumberWrongAnswers = answeredSteps.Count(s => s.Answer.AnswerredCorrectly == AnswerCorrectness.False);
            NumberNotAnswered = LearningSession.Steps.Count(s => s.AnswerState == StepAnswerState.Skipped || s.AnswerState == StepAnswerState.NotViewedOrAborted);

            NumberCorrectPercentage = (int)Math.Round(NumberCorrectAnswers / (float)NumberSteps * 100);
            NumberWrongAnswersPercentage = (int)Math.Round(NumberWrongAnswers / (float)NumberSteps * 100);
            NumberNotAnsweredPercentage = (int)Math.Round(NumberNotAnswered / (float)NumberSteps * 100);
        }
    }
}
