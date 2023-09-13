using System.Linq;
using Microsoft.AspNetCore.Mvc;

public class LearningSessionStoreController: BaseController
{
    private readonly LearningSessionCreator _learningSessionCreator;
    private readonly PermissionCheck _permissionCheck;
    private readonly LearningSessionCache _learningSessionCache;

    public LearningSessionStoreController(LearningSessionCreator learningSessionCreator,
        PermissionCheck permissionCheck,
        SessionUser sessionUser, LearningSessionCache learningSessionCache) : base(sessionUser)
    {
        _learningSessionCreator = learningSessionCreator;
        _permissionCheck = permissionCheck;
        _learningSessionCache = learningSessionCache;
    }
    [HttpPost]
    public JsonResult NewSession([FromBody]LearningSessionConfig config)
    {
        var newSession = _learningSessionCreator.BuildLearningSession(config);
        _learningSessionCache.AddOrUpdate(newSession);

        var learningSession = _learningSessionCache.GetLearningSession();

        if (learningSession is { Steps: { Count: > 0 } })
        {
            var firstStep = learningSession.Steps.First();
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
                firstStep = new
                {
                    state = firstStep.AnswerState,
                    id = firstStep.Question.Id,
                    index = 0
                },
                answerHelp = learningSession.Config.AnswerHelp,
                isInTestMode = learningSession.Config.IsInTestMode
            });
        }

        return Json(new
        {
            success = false
        });
    }

    [HttpPost]
    public JsonResult NewSessionWithJumpToQuestion(LearningSessionConfig config, int id)
    {
        var allQuestions = EntityCache.GetCategory(config.CategoryId).GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId);
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
    public JsonResult GetLastStepInQuestionList(int index)
    {
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
    public JsonResult LoadSpecificQuestion(int index)
    {
        var learningSession = _learningSessionCache.GetLearningSession();
        learningSession.LoadSpecificQuestion(index);

        return Json(new
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
                index = index,
                isLastStep = learningSession.TestIsLastStep()
            },
        });
    }

    [HttpPost]
    public JsonResult SkipStep(int index)
    {
        var learningSession = _learningSessionCache.GetLearningSession();
        if (learningSession.CurrentIndex == index)
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
    public JsonResult LoadSteps()
    {
        var learningSession = _learningSessionCache.GetLearningSession();
        return Json(learningSession.Steps.Select((s, index) => new StepResult
        {
            id = s.Question.Id,
            state = s.AnswerState,
            index = index,
            isLastStep = learningSession.Steps.Last() == s
        }).ToArray());
    }

    public class StepResult
    {
        public AnswerState state;
        public int id;
        public int index;
        public bool isLastStep;
    }

}