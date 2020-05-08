using System;
using System.Collections.Generic;
using System.Windows.Forms;

public class LearningSessionNew 
{
    public IList<LearningSessionStepNew> Steps;

    public int CurrentIndex { get; set; }

    public LearningSessionConfig Config;
    public int Pager;

    public LearningSessionStepNew CurrentStep;
    public LearningSessionStepNew NextStep;
    public bool HaveNextStep;
    public int UserId;
    public bool IsLoggedIn; 

    public LearningSessionNew(List<LearningSessionStepNew> learningSessionSteps, LearningSessionConfig config, int currentStep)
    {
        Steps = learningSessionSteps;
        UserId = config.UserId; 
        IsLoggedIn =  UserId != -1;
        
        HaveListNextStep(currentStep, learningSessionSteps.Count);
    }

    public void SetCorrectAnswer(int currentIndex)
    {
        var currentStep = Steps[currentIndex]; 
        CurrentIndex = currentIndex;

        currentStep.AnswerState= AnswerStateNew.CorrectAnswered;

        HaveListNextStep(currentIndex, Steps.Count);
    }

    public void SetFalseAnswer(int currentIndex)
    {
        CurrentIndex = currentIndex;
        var currentStep = Steps[currentIndex];

        if (IsLoggedIn)
        {
            Steps.Add(currentStep);
            HaveNextStep = true; 
        }
        else
        {
            HaveListNextStep(currentIndex, Steps.Count);
        }

        currentStep.AnswerState = AnswerStateNew.FalseAnswered;
    }

    public void SetSkipAnswer(int currentIndex)
    {
        CurrentIndex = currentIndex;
        var currentStep = Steps[currentIndex];

        currentStep.AnswerState = AnswerStateNew.Skip; 

    }

    private void HaveListNextStep(int currentStep, int ListCount)
    {
        HaveNextStep = ListCount - currentStep > 1; 
    }
}