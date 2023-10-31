using System.Linq;
using HelperClassesControllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

//using Newtonsoft.Json;

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
        var newSession = _learningSessionCreator.BuildLearningSession(config);
        _learningSessionCache.AddOrUpdate(newSession);

        var learningSession = _learningSessionCache.GetLearningSession();

        if (learningSession is { Steps: { Count: > 0 } })
        {
            var firstStep = learningSession.Steps.First();
            var result = new
            {
                success = true,
                steps = learningSession.Steps.Select((s, index) => new
                {
                    id = s.Question.Id,
                    state = s.AnswerState,
                    index = index
                }).ToArray(),
                activeQuestionCount = learningSession.Steps.DistinctBy(s => s.Question).Count(),
                firstStep = new
                {
                    state = firstStep.AnswerState,
                    id = firstStep.Question.Id,
                    index = 0
                },
                answerHelp = learningSession.Config.AnswerHelp,
                isInTestMode = learningSession.Config.IsInTestMode
            };
            return Json(result);
        }

        return Json(new
        {
            success = false
        });
    }

    [HttpPost]
    public JsonResult NewSessionWithJumpToQuestion([FromBody] NewSessionWithJumpToQuestionData data)
    {
        var config = data.Config;
        var id = data.Id;

        var category = EntityCache.GetCategory(config.CategoryId);
        var allQuestions = category.GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId);
        allQuestions = allQuestions.Where(q => q.Id > 0 && _permissionCheck.CanView(q)).ToList();
        if (allQuestions.IndexOf(q => q.Id == id) < 0)
            return Json(new
            {
                success = false,
                message = "questionDoesntExistInTopic"
            });

        if (!_permissionCheck.CanViewQuestion(id))
            return Json(new
            {
                success = false,
                message = "private"
            });

        var newSession = _learningSessionCreator.BuildLearningSessionWithSpecificQuestion(config, id, allQuestions);

        if (newSession == null)
            return Json(new
            {
                success = false,
                message = "questionNotInFilter"
            });

        _learningSessionCache.AddOrUpdate(newSession);

        var learningSession = _learningSessionCache.GetLearningSession();

        var index = learningSession.Steps.IndexOf(s => s.Question.Id == id);
        learningSession.LoadSpecificQuestion(index);

        return Json(new
        {
            success = true,
            steps = learningSession.Steps.Select((s, index) => new
            {
                id = s.Question.Id,
                state = s.AnswerState,
                index = index
            }).ToArray(),
            activeQuestionCount = learningSession.Steps.DistinctBy(s => s.Question).Count(),
            currentStep = new
            {
                state = learningSession.CurrentStep.AnswerState,
                id = learningSession.CurrentStep.Question.Id,
                index = index,
                isLastStep = learningSession.TestIsLastStep()
            },
            answerHelp = learningSession.Config.AnswerHelp,
            isInTestMode = learningSession.Config.IsInTestMode
        });

    }

    [HttpGet]
    public JsonResult GetLastStepInQuestionList([FromHeader] Counter counter)
    {
        var learningSession = _learningSessionCache.GetLearningSession();

        if (learningSession != null)
        {
            learningSession.LoadSpecificQuestion(counter.Index);

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
                    id = learningSession.Steps[counter.Index].Question.Id,
                    state = AnswerState.Unanswered,
                    index = counter.Index
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
        var learningSession = _learningSessionCache.GetLearningSession();
        if (learningSession != null)
        {
            var firstUnansweredStep = learningSession.Steps.First(s => s.AnswerState != AnswerState.Unanswered);
            return Json(new
            {
                success = true,
                steps = learningSession.Steps.Select((s, index) => new StepResult
                {
                    id = s.Question.Id,
                    state = s.AnswerState,
                    index = index,
                    isLastStep = learningSession.Steps.Last() == s
                }).ToArray(),
                activeQuestionCount = learningSession.Steps.DistinctBy(s => s.Question).Count(),
                firstUnansweredStep = new StepResult
                {
                    state = firstUnansweredStep.AnswerState,
                    id = firstUnansweredStep.Question.Id,
                    index = learningSession.Steps.IndexOf(s => s == firstUnansweredStep),
                    isLastStep = learningSession.Steps.Last() == firstUnansweredStep
                }

            });
        }

        return Json(new
        {
            success = false
        });
    }

    [HttpPost]
    public IActionResult LoadSpecificQuestion([FromBody] Counter counter)
    {
        if (counter.Index == -1)
        {
            return Json(""); 
        }

        var learningSession = _learningSessionCache.GetLearningSession();
        learningSession.LoadSpecificQuestion(counter.Index);

        var json = JsonConvert.SerializeObject(new
        {
            steps = learningSession.Steps.Select((s, index) => new StepResult
            {
                id = s.Question.Id,
                state = s.AnswerState,
                index = index,
                isLastStep = learningSession.Steps.Last() == s
            }).ToArray(),
            currentStep = new StepResult
            {
                state = learningSession.CurrentStep.AnswerState,
                id = learningSession.CurrentStep.Question.Id,
                index = counter.Index,
                isLastStep = learningSession.TestIsLastStep()
            },
        });

        return Content(json, "application/json");
    }

    [HttpPost]
    public JsonResult SkipStep([FromBody] Counter counter)
    {
        var learningSession = _learningSessionCache.GetLearningSession();
        if (learningSession.CurrentIndex == counter.Index)
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


namespace HelperClassesControllers
{
    public class NewSessionWithJumpToQuestionData
    {
        public LearningSessionConfig Config { get; set; }
        public int Id { get; set; }
    }

    public class Counter
    {
        public int Index { get; set; }
    }

}