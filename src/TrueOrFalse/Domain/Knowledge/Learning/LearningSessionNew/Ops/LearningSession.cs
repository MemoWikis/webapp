﻿using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class LearningSession
{
    public IList<LearningSessionStep> Steps;
    public LearningSessionConfig Config;
    public int Pager;

    public int CurrentIndex { get; private set; }
    public bool IsLastStep { get; private set; }
    public LearningSessionStep CurrentStep => Steps[CurrentIndex];
    public string UrlName = "";

    public User User;
    public bool IsLoggedIn;
    public Guid QuestionViewGuid;

    public LearningSession(List<LearningSessionStep> learningSessionSteps, LearningSessionConfig config)
    {
        Steps = learningSessionSteps;
        var userCacheItem = UserCache.GetItem(config.CurrentUserId);
        User = userCacheItem.User;
        IsLoggedIn = config.CurrentUserId != -1;
        Config = config;
        Config.Category = EntityCache.GetCategoryCacheItem(Config.CategoryId);
    }

    public bool AddAnswer(AnswerQuestionResult answer)
    {
        CurrentStep.AnswerState = answer.IsCorrect ? AnswerState.Correct : AnswerState.False;
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
        CurrentStep.AnswerState = AnswerState.Skipped;
        IsLastStep = TestIsLastStep();

        if (!IsLastStep)
            CurrentIndex++;
    }

    public void LoadSpecificQuestion(int index)
    {
        if (index > CurrentIndex)
        {
            for (int i = CurrentIndex; i < index; i++)
            {
                Steps[i].AnswerState = AnswerState.Skipped;
            }
        }

        CurrentIndex = index; 
    }

    public void ShowSolution()
    {
        ReAddCurrentStepToEnd();
    }

    private bool ReAddCurrentStepToEnd()
    {
        if (LimitForThisQuestionHasBeenReached(CurrentStep) || LimitForNumberOfRepetitionsHasBeenReached() ||
            Config.IsInTestMode || Config.IsAnonymous() || CurrentStep.AnswerState == AnswerState.Correct || !Config.Repititions)
        {
            return false;
        }

        var step = new LearningSessionStep(CurrentStep.Question);
        Steps.Add(step);
        return true; 
    }

    private bool TestIsLastStep()
    {
        return CurrentIndex == Steps.Count - 1;
    }


    public virtual bool LimitForThisQuestionHasBeenReached(LearningSessionStep step)
    {
        return Steps.Count(s => s.Question == step.Question) >= 3;
    }

    public virtual bool LimitForNumberOfRepetitionsHasBeenReached()
    {
        return Steps.Count >= Steps.Select(s => s.Question).Distinct().Count() * 2;
    }

    public int TotalPossibleQuestions()
    {
        if (!Config.InWishknowledge)
            return EntityCache.GetCategoryCacheItem(Config.CategoryId).CountQuestions;

        if (Config.InWishknowledge)
            return User.WishCountQuestions;

        throw new Exception("unknown session type");
    }

    public void SetCurrentStepAsCorrect()
    {
        CurrentStep.AnswerState = AnswerState.Correct;
        DeleteLastStep();
    }
    public void DeleteLastStep()
    {
        if (!Config.IsAnonymous() && !Config.IsInTestMode)
            Steps.RemoveAt(Steps.Count - 1);
    }
}