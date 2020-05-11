using System.Collections.Generic;
using FluentNHibernate.Utils;

public class LearningSessionNew
{
    public IList<LearningSessionStepNew> Steps;
    public LearningSessionConfig Config;
    public int Pager;

    public int CurrentIndex { get; private set; }
    public LearningSessionStepNew CurrentStep => Steps[CurrentIndex];

    public int UserId;
    public bool IsLoggedIn; 


    public LearningSessionNew(List<LearningSessionStepNew> learningSessionSteps, LearningSessionConfig config)
    {
        Steps = learningSessionSteps;
        UserId = config.UserId; 
        IsLoggedIn =  UserId != -1;
        Config = config; 
    }

    public void AddAnswer(Answer answer)
    {
        CurrentStep.AnswerState = answer.AnsweredCorrectly() ? AnswerStateNew.Correct : AnswerStateNew.False;
        
        if(Config.ReAddStepsToEnd() && !answer.AnsweredCorrectly())
            ReAddCurrentStepToEnd();
    }

    public void NextStep()
    {
        CurrentIndex++; 
        //var question = Steps[CurrentIndex].Question;
        //return new AnswerBody(question);
    }

    public void SkipStep()
    {
        CurrentStep.AnswerState = AnswerStateNew.Skipped;

        if (Config.ReAddStepsToEnd())
            ReAddCurrentStepToEnd();

        CurrentIndex++;
    }

    private void ReAddCurrentStepToEnd()
    {
        var step = new LearningSessionStepNew(CurrentStep.Question);
        Steps.Add(step);
    }
}