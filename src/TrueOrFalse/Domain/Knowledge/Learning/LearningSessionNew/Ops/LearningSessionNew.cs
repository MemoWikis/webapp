using System;
using System.Collections.Generic;
using System.Linq;
using SolrNet.Commands.Cores;

[Serializable]
public class LearningSessionNew
{
    public IList<LearningSessionStepNew> Steps;
    public LearningSessionConfig Config;
    public int Pager;

    public int CurrentIndex { get; private set; }
    public bool IsLastStep { get; private set; }
    public LearningSessionStepNew CurrentStep => Steps[CurrentIndex];
    public string UrlName = "";

    public User User;
    public bool IsLoggedIn;


    public LearningSessionNew(List<LearningSessionStepNew> learningSessionSteps, LearningSessionConfig config)
    {
        Steps = learningSessionSteps;
        var userCashItem = UserCache.GetItem(config.UserId);
        User = userCashItem.User;
        IsLoggedIn = config.UserId != -1;
        Config = config;
        Config.Category = EntityCache.GetCategory(Config.CategoryId);
    }

    public bool AddAnswer(AnswerQuestionResult answer)
    {
        CurrentStep.AnswerState = answer.IsCorrect ? AnswerStateNew.Correct : AnswerStateNew.False;
        return ReAddCurrentStepToEnd();
    }

    public void NextStep()
    {
        IsLastStep = TestIsLastStep();

        if (!IsLastStep)
            CurrentIndex++;
    }

    public void SkipStep()
    {
        CurrentStep.AnswerState = AnswerStateNew.Skipped;

        if (Config.ReAddStepsToEnd())
            ReAddCurrentStepToEnd();

        IsLastStep = TestIsLastStep();

        if (!IsLastStep)
            CurrentIndex++;
    }

    public void ShowSolution()
    {
        ReAddCurrentStepToEnd();

    }

    private bool ReAddCurrentStepToEnd()
    {
        if (LimitForThisQuestionHasBeenReached(CurrentStep) || LimitForNumberOfRepetitionsHasBeenReached() || Config.IsInTestMode || Config.IsAnonymous() || CurrentStep.AnswerState == AnswerStateNew.Correct)
            return false;

        var step = new LearningSessionStepNew(CurrentStep.Question);
        Steps.Add(step);
        return true; 
    }

    private bool TestIsLastStep()
    {
        return CurrentIndex == Steps.Count - 1;
    }


    public virtual bool LimitForThisQuestionHasBeenReached(LearningSessionStepNew step)
    {
        return Steps.Count(s => s.Question == step.Question) >= 3;
    }

    public virtual bool LimitForNumberOfRepetitionsHasBeenReached()
    {
        return Steps.Count >= Steps.Select(s => s.Question).Distinct().Count() * 2;
    }

    public int TotalPossibleQuestions
    {
        get
        {
            if (!Config.IsWishSession)
                return EntityCache.GetCategory(Config.CategoryId).CountQuestions;

            if (Config.IsWishSession)
                return User.WishCountQuestions;

            throw new Exception("unknown session type");
        }
    }

    public void CountAsCorrect()
    {
        CurrentStep.AnswerState = AnswerStateNew.Correct;
        DeleteLastStep();
    }
    public void DeleteLastStep()
    {
        if (!Config.IsAnonymous() && !Config.IsInTestMode)
            Steps.RemoveAt(Steps.Count - 1);
    }


}