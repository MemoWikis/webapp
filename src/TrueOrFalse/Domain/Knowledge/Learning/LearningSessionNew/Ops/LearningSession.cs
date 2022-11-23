using System;
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

    public UserEntityCacheItem User;
    public bool IsLoggedIn;
    public Guid QuestionViewGuid;
    public QuestionCounter QuestionCounter;
    public LearningSession(List<LearningSessionStep> learningSessionSteps, LearningSessionConfig config)
    {
        Steps = learningSessionSteps;
        var userId = config.CurrentUserId == 0 ? SessionUser.UserId : config.CurrentUserId;
        var userCacheItem = SessionUserCache.GetItem(userId);
        User = userCacheItem;
        IsLoggedIn = config.CurrentUserId != -1;
        Config = config;
        Config.Category = EntityCache.GetCategory(Config.CategoryId);
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
        if (LimitForThisQuestionHasBeenReached(CurrentStep) || 
            LimitForNumberOfRepetitionsHasBeenReached() || 
            Config.IsAnonymous() || 
            CurrentStep.AnswerState == AnswerState.Correct || 
            Config.Repetition == RepetitionType.None)
            return false;

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
            return EntityCache.GetQuestionsForCategory(Config.CategoryId).Count;
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

public class QuestionCounter
{
    public int InWuwi;
    public int NotInWuwi;
    public int CreatedByCurrentUser;
    public int NotCreatedByCurrentUser;
    public int Private;
    public int Public;
    public int NotLearned;
    public int NeedsLearning;
    public int NeedsConsolidation;
    public int Solid;
    public int Max;
}