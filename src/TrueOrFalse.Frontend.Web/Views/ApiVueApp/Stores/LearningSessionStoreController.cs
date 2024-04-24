using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class LearningSessionStoreController(
    LearningSessionCreator _learningSessionCreator,
    LearningSessionCache _learningSessionCache) : Controller
{
    public record struct LearningSessionResult()
    {
        public int Index { get; set; } = 0;

        public LearningSessionCreator.Step[] Steps { get; set; } =
            Array.Empty<LearningSessionCreator.Step>();

        public LearningSessionCreator.Step? CurrentStep { get; set; } = null;
        public int ActiveQuestionCount { get; set; } = 0;
        public bool AnswerHelp { get; set; } = true;
        public bool IsInTestMode { get; set; } = false;
        public string MessageKey { get; set; } = null;
    }

    [HttpPost]
    public LearningSessionResult NewSession([FromBody] LearningSessionConfig config)
    {
        var data = _learningSessionCreator.GetLearningSessionResult(config);
        return new LearningSessionResult
        {
            MessageKey = data.MessageKey,
            ActiveQuestionCount = data.ActiveQuestionCount,
            AnswerHelp = data.AnswerHelp,
            CurrentStep = data.CurrentStep,
            Index = data.Index,
            IsInTestMode = data.IsInTestMode,
            Steps = data.Steps
        };
    }

    public readonly record struct NewSessionWithJumpToQuestionData(
        LearningSessionConfig Config,
        int Id);

    [HttpPost]
    public LearningSessionResult NewSessionWithJumpToQuestion(
        [FromBody] NewSessionWithJumpToQuestionData data)
    {
        var resultData = _learningSessionCreator.GetLearningSessionResult(data.Config, data.Id);
        return new LearningSessionResult
        {
            MessageKey = resultData.MessageKey,
            ActiveQuestionCount = resultData.ActiveQuestionCount,
            AnswerHelp = resultData.AnswerHelp,
            CurrentStep = resultData.CurrentStep,
            Index = resultData.Index,
            IsInTestMode = resultData.IsInTestMode,
            Steps = resultData.Steps
        };
    }

    public readonly record struct LastStepInQuestionListResult(
        bool Success,
        Step[] Steps,
        int ActivityQuestionCount,
        Step LastQuestionInList,
        int ActiveQuestionCount);

    public readonly record struct Step(
        int Id,
        AnswerState State,
        int ActiveQuestionCount,
        int Index);

    [HttpGet]
    public LastStepInQuestionListResult GetLastStepInQuestionList([FromRoute] int id)
    {
        var index = id;
        var learningSession = _learningSessionCache.GetLearningSession();

        if (learningSession != null)
        {
            learningSession.LoadSpecificQuestion(index);

            return new LastStepInQuestionListResult
            {
                Success = true,
                Steps = learningSession.Steps.Select((s, i) => new Step
                {
                    Id = s.Question.Id,
                    State = s.AnswerState,
                    Index = i
                }).ToArray(),
                ActiveQuestionCount = learningSession.Steps.DistinctBy(s => s.Question).Count(),
                LastQuestionInList = new Step
                {
                    Id = learningSession.Steps[index].Question.Id,
                    State = AnswerState.Unanswered,
                    Index = index
                }
            };
        }

        return new LastStepInQuestionListResult
        {
            Success = false
        };
    }

    [HttpGet]
    public LearningSessionResult GetCurrentSession()
    {
        var data = _learningSessionCreator.GetLearningSessionResult();
        return new LearningSessionResult
        {
            MessageKey = data.MessageKey,
            ActiveQuestionCount = data.ActiveQuestionCount,
            AnswerHelp = data.AnswerHelp,
            CurrentStep = data.CurrentStep,
            Index = data.Index,
            IsInTestMode = data.IsInTestMode,
            Steps = data.Steps
        };
    }

    public readonly record struct LoadSpecificQuestionJson(int index);

    [HttpPost]
    public LearningSessionResult LoadSpecificQuestion(
        [FromBody] LoadSpecificQuestionJson json)
    {
        var data = _learningSessionCreator.GetStep(json.index);
        return new LearningSessionResult
        {
            MessageKey = data.MessageKey,
            ActiveQuestionCount = data.ActiveQuestionCount,
            AnswerHelp = data.AnswerHelp,
            CurrentStep = data.CurrentStep,
            Index = data.Index,
            IsInTestMode = data.IsInTestMode,
            Steps = data.Steps
        };
    }

    public readonly record struct SkipStepJson(int index);

    [HttpPost]
    public StepResult? SkipStep([FromBody] SkipStepJson json)
    {
        var learningSession = _learningSessionCache.GetLearningSession();
        if (learningSession.CurrentIndex == json.index)
        {
            learningSession.SkipStep();
            new StepResult
            {
                State = learningSession.CurrentStep.AnswerState,
                Id = learningSession.CurrentStep.Question.Id,
                Index = learningSession.CurrentIndex,
                IsLastStep = learningSession.TestIsLastStep()
            };
        }

        return null;
    }

    [HttpGet]
    public IActionResult LoadSteps()
    {
        var learningSession = _learningSessionCache.GetLearningSession();
        var result = learningSession.Steps.Select((s, index) => new StepResult
        {
            Id = s.Question.Id,
            State = s.AnswerState,
            Index = index,
            IsLastStep = learningSession.Steps.Last() == s
        }).ToArray();

        var serializedResult = JsonConvert.SerializeObject(result);

        return Content(serializedResult, "application/json");
    }

    public record struct StepResult(AnswerState State, int Id, int Index, bool IsLastStep);
}