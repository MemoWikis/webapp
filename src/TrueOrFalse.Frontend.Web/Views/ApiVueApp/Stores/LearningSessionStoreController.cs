using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class LearningSessionStoreController(
    LearningSessionCreator _learningSessionCreator,
    LearningSessionCache _learningSessionCache) : Controller
{
    [HttpPost]
    public LearningSessionCreator.LearningSessionResult NewSession(
        [FromBody] LearningSessionConfig config)
    {
        return _learningSessionCreator.GetLearningSessionResult(config);
    }

    public readonly record struct NewSessionWithJumpToQuestionData(
        LearningSessionConfig Config,
        int Id);

    [HttpPost]
    public LearningSessionCreator.LearningSessionResult NewSessionWithJumpToQuestion(
        [FromBody] NewSessionWithJumpToQuestionData data)
    {
        return _learningSessionCreator.GetLearningSessionResult(data.Config, data.Id);
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
    public LearningSessionCreator.LearningSessionResult GetCurrentSession()
    {
        return _learningSessionCreator.GetLearningSessionResult();
    }

    public readonly record struct LoadSpecificQuestionJson(int index);

    [HttpPost]
    public LearningSessionCreator.LearningSessionResult LoadSpecificQuestion(
        [FromBody] LoadSpecificQuestionJson json)
    {
        return _learningSessionCreator.GetStep(json.index);
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
                state = learningSession.CurrentStep.AnswerState,
                id = learningSession.CurrentStep.Question.Id,
                index = learningSession.CurrentIndex,
                isLastStep = learningSession.TestIsLastStep()
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
            id = s.Question.Id,
            state = s.AnswerState,
            index = index,
            isLastStep = learningSession.Steps.Last() == s
        }).ToArray();

        var serializedResult = JsonConvert.SerializeObject(result);

        return Content(serializedResult, "application/json");
    }
}

public class StepResult
{
    public AnswerState state { get; set; }
    public int id { get; set; }
    public int index { get; set; }
    public bool isLastStep { get; set; }
}