using System.Collections.Generic;
using System.Linq;

[Serializable]
public class LearningSession

{
    public IList<LearningSessionStep> Steps;
    public LearningSessionConfig Config;
    private readonly SessionUser _sessionUser;
    public UserCacheItem User;
    public bool IsLoggedIn;
    public QuestionCounter QuestionCounter;

    public LearningSession(List<LearningSessionStep> learningSessionSteps, LearningSessionConfig config, SessionUser sessionUser)
    {
        Steps = learningSessionSteps;
        var userId = config.CurrentUserId == 0 ? _sessionUser.UserId : config.CurrentUserId;
        User = EntityCache.GetUserById(userId);
        IsLoggedIn = userId > 0;
        Config = config;
        _sessionUser = sessionUser;
        Config.Category = EntityCache.GetCategory(Config.CategoryId);
    }

    public LearningSessionStep CurrentStep => Steps[CurrentIndex];

    public int CurrentIndex { get; private set; }
    public bool IsLastStep { get; private set; }

    public bool AddAnswer(AnswerQuestionResult answer)
    {
        CurrentStep.AnswerState = answer.IsCorrect ? AnswerState.Correct : AnswerState.False;
        return ReAddCurrentStepToEnd();
    }

    public void DeleteLastStep()
    {
        if (!Config.IsAnonymous() && !Config.IsInTestMode)
        {
            Steps.RemoveAt(Steps.Count - 1);
        }
    }

    public virtual bool LimitForNumberOfRepetitionsHasBeenReached()
    {
        return Steps.Count >= Steps.Select(s => s.Question).Distinct().Count() * 2;
    }


    public virtual bool LimitForThisQuestionHasBeenReached(LearningSessionStep step)
    {
        return Steps.Count(s => s.Question == step.Question) >= 3;
    }

    public void LoadSpecificQuestion(int index)
    {
        if (index > CurrentIndex)
        {
            for (var i = CurrentIndex; i < index; i++)
            {
                if (Steps[i].AnswerState == AnswerState.Unanswered)
                {
                    Steps[i].AnswerState = AnswerState.Skipped;
                }
            }
        }

        CurrentIndex = index;
    }

    public void NextStep()
    {
        IsLastStep = TestIsLastStep();

        if (!IsLastStep)
        {
            CurrentIndex++;
        }
    }

    public void SetCurrentStepAsCorrect()
    {
        CurrentStep.AnswerState = AnswerState.Correct;
        DeleteLastStep();
    }

    public void SkipStep()
    {
        CurrentStep.AnswerState = AnswerState.Skipped;
        IsLastStep = TestIsLastStep();

        if (!IsLastStep)
        {
            CurrentIndex++;
        }
    }

    public bool TestIsLastStep()
    {
        return CurrentIndex == Steps.Count - 1;
    }

    private bool ReAddCurrentStepToEnd()
    {
        if (LimitForThisQuestionHasBeenReached(CurrentStep) ||
            LimitForNumberOfRepetitionsHasBeenReached() ||
            Config.IsAnonymous() ||
            CurrentStep.AnswerState == AnswerState.Correct ||
            Config.Repetition == RepetitionType.None)
        {
            return false;
        }

        var step = new LearningSessionStep(CurrentStep.Question);
        Steps.Add(step);
        return true;
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