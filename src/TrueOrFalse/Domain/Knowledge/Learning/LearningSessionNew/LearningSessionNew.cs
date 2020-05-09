using System.Collections.Generic;

public class LearningSessionNew
{
    public IList<LearningSessionStepNew> Steps;
    public int CurrentIndex { get; set; }
    public LearningSessionConfig Config;
    public int Pager;
    public LearningSessionStepNew CurrentStep;
    public int UserId;
    public bool IsLoggedIn; 

    public LearningSessionNew(List<LearningSessionStepNew> learningSessionSteps, LearningSessionConfig config)
    {
        Steps = learningSessionSteps;
        UserId = config.UserId; 
        IsLoggedIn =  UserId != -1;
    }

    public void SetAnswer(Answer answer)
    {
        Steps[CurrentIndex].AnswerState = answer.AnsweredCorrectly() ? AnswerStateNew.Correct : AnswerStateNew.False;
        AddCurrentlyStepToStepsWhenAnswerFalseOrSkipped(answer);

        // Save Questionview(Answer answer) in Database
    }

    public void NextStep()
    {
        CurrentIndex++; 
        //var question = Steps[CurrentIndex].Question;
        //return new AnswerBody(question);
    }

    public void SkipStep()
    {
        Steps[CurrentIndex].AnswerState = AnswerStateNew.Skipped;
        AddCurrentlyStepToStepsWhenAnswerFalseOrSkipped(null);
        CurrentIndex++; 
    }

    private void AddCurrentlyStepToStepsWhenAnswerFalseOrSkipped(Answer answer)
    {
        var step = Steps[CurrentIndex];
        step.AnswerState = AnswerStateNew.Unanswered; 
        if (answer == null || UserId != -1 && !answer.AnsweredCorrectly())
        {
            Steps.Add(step);
        }
    }
}