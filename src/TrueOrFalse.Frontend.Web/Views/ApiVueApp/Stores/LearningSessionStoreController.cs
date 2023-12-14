using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
public class LearningSessionStoreController : BaseController
{
    private readonly LearningSessionCreator _learningSessionCreator;
    private readonly PermissionCheck _permissionCheck;
    private readonly LearningSessionCache _learningSessionCache;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LearningSessionStoreController(LearningSessionCreator learningSessionCreator,
        PermissionCheck permissionCheck,
        SessionUser sessionUser,
        LearningSessionCache learningSessionCache,
        IHttpContextAccessor httpContextAccessor) : base(sessionUser)
    {
        _learningSessionCreator = learningSessionCreator;
        _permissionCheck = permissionCheck;
        _learningSessionCache = learningSessionCache;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    public JsonResult NewSession([FromBody] LearningSessionConfig config)
    {
        return Json(_learningSessionCreator.GetLearningSessionResult(config));
    }

    public readonly record struct NewSessionWithJumpToQuestionData(LearningSessionConfig Config, int Id);
    [HttpPost]
    public JsonResult NewSessionWithJumpToQuestion([FromBody] NewSessionWithJumpToQuestionData data)
    {
        return Json(_learningSessionCreator.GetLearningSessionResult(data.Config, data.Id));
    }

    [HttpGet]       
    public JsonResult GetLastStepInQuestionList([FromRoute] int id)
    {
        var index = id;
        var learningSession = _learningSessionCache.GetLearningSession();

        if (learningSession != null)
        {
            learningSession.LoadSpecificQuestion(index);

            return Json(new
            {
                success = true,
                steps = learningSession.Steps.Select((s, i) => new
                {
                    id = s.Question.Id,
                    state = s.AnswerState,
                    index = i
                }).ToArray(),
                activeQuestionCount = learningSession.Steps.DistinctBy(s => s.Question).Count(),
                lastQuestionInList = new
                {
                    id = learningSession.Steps[index].Question.Id,
                    state = AnswerState.Unanswered,
                    index = index
                }
            });
        }

        return Json(new
        {
            success = false
        });
    }

    [HttpGet]
    public JsonResult GetCurrentSession()
    {
        return Json(_learningSessionCreator.GetLearningSessionResult());
    }

    public readonly record struct LoadSpecificQuestionJson(int index);
    [HttpPost]
    public JsonResult LoadSpecificQuestion([FromBody] LoadSpecificQuestionJson json)
    {
        return Json(_learningSessionCreator.GetStep(json.index));
    }

    public readonly record struct SkipStepJson(int index);
    [HttpPost]
    public JsonResult SkipStep([FromBody] SkipStepJson json)
    {
        var learningSession = _learningSessionCache.GetLearningSession();
        if (learningSession.CurrentIndex == json.index)
        {
            learningSession.SkipStep();
            return Json(new StepResult
            {
                state = learningSession.CurrentStep.AnswerState,
                id = learningSession.CurrentStep.Question.Id,
                index = learningSession.CurrentIndex,
                isLastStep = learningSession.TestIsLastStep()
            });
        }

        return Json(null);
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