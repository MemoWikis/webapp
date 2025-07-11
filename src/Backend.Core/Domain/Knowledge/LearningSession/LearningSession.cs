﻿[Serializable]
public class LearningSession(
    List<LearningSessionStep> _learningSessionSteps,
    LearningSessionConfig _config)
{
    public List<LearningSessionStep> Steps { get; set; } = _learningSessionSteps;
    public LearningSessionConfig Config { get; set; } = _config;

    public QuestionCounter QuestionCounter;

    public LearningSessionStep? CurrentStep =>
        CurrentIndex <= 0 ? Steps.Any() ? Steps[0] : null : Steps[CurrentIndex];

    public int CurrentIndex { get; private set; }
    public bool IsLastStep { get; private set; }

    public bool AddAnswer(AnswerQuestionResult answer)
    {
        if (CurrentStep == null)
        {
            Log.Error("CurrentStep in LearningSession is null");
            throw new NullReferenceException();
        }

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
        if (CurrentStep == null)
            return;

        CurrentStep.AnswerState = AnswerState.Correct;
        DeleteLastStep();
    }

    public void SkipStep()
    {
        if (CurrentStep == null)
            return;

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
        if (CurrentStep == null)
            return false;

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