public class LearningSessionStoreController(
    LearningSessionCreator _learningSessionCreator,
    LearningSessionCurrent _learningSessionCurrent,
    LearningSessionCache _learningSessionCache,
    PermissionCheck _permissionCheck) : ApiBaseController
{
    public record struct LearningSessionResponse()
    {
        public int Index { get; set; } = 0;

        public global::Step[] Steps { get; set; } = [];

        public global::Step? CurrentStep { get; set; } = null;
        public int ActiveQuestionCount { get; set; } = 0;
        public bool AnswerHelp { get; set; } = true;
        public bool IsInTestMode { get; set; } = false;
        public string MessageKey { get; set; } = null;
        public bool Success { get; set; } = false;
    }

    public sealed record LearningSessionConfigRequest
    {
        public int PageId { get; init; }
        public int MaxQuestionCount { get; init; } = 0;
        public int CurrentUserId { get; init; } = 0;
        public bool IsInTestMode { get; init; }

        public QuestionOrder QuestionOrder { get; init; } = QuestionOrder.SortByEasiest;
        public bool AnswerHelp { get; init; }
        public RepetitionType Repetition { get; init; } = RepetitionType.None;

        // Filter-Flags (alle default true)
        public bool InWishKnowledge { get; init; } = true;
        public bool NotInWishKnowledge { get; init; } = true;
        public bool CreatedByCurrentUser { get; init; } = true;
        public bool NotCreatedByCurrentUser { get; init; } = true;
        public bool PrivateQuestions { get; init; } = true;
        public bool PublicQuestions { get; init; } = true;
        public bool NotLearned { get; init; } = true;
        public bool NeedsLearning { get; init; } = true;
        public bool NeedsConsolidation { get; init; } = true;
        public bool Solid { get; init; } = true;
    }


    [HttpPost]
    public LearningSessionResponse NewSession([FromBody] LearningSessionConfigRequest learningSessionConfigRequest)
    {
        var config = learningSessionConfigRequest.ToEntity();

        if (config == null || config.PageId < 1 || !_permissionCheck.CanViewPage(config.PageId))
            return new LearningSessionResponse
            {
                MessageKey = FrontendMessageKeys.Error.Default,
                Success = false
            };

        var data = _learningSessionCreator.GetLearningSessionResult(config);
        return new LearningSessionResponse
        {
            MessageKey = data.MessageKey,
            ActiveQuestionCount = data.ActiveQuestionCount,
            AnswerHelp = data.AnswerHelp,
            CurrentStep = data.CurrentStep,
            Index = data.Index,
            IsInTestMode = data.IsInTestMode,
            Steps = data.Steps,
            Success = true
        };
    }

    public readonly record struct NewSessionWithJumpToQuestionData(
        LearningSessionConfig Config,
        int Id);

    [HttpPost]
    public LearningSessionResponse NewSessionWithJumpToQuestion(
        [FromBody] NewSessionWithJumpToQuestionData data)
    {
        if (data.Config == null || data.Config.PageId < 1 || !_permissionCheck.CanViewPage(data.Config.PageId))
            return new LearningSessionResponse
            {
                MessageKey = FrontendMessageKeys.Error.Default,
                Success = false
            };

        var resultData = _learningSessionCreator.GetLearningSessionResult(data.Config, data.Id);
        return new LearningSessionResponse
        {
            MessageKey = resultData.MessageKey,
            ActiveQuestionCount = resultData.ActiveQuestionCount,
            AnswerHelp = resultData.AnswerHelp,
            CurrentStep = resultData.CurrentStep,
            Index = resultData.Index,
            IsInTestMode = resultData.IsInTestMode,
            Steps = resultData.Steps,
            Success = true
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
        var learningSession = _learningSessionCache.GetLearningSession(log: false);

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
    public LearningSessionResponse GetCurrentSession()
    {
        var data = _learningSessionCurrent.GetCurrentSession();
        return new LearningSessionResponse
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

    [HttpPost]
    public LearningSessionResponse LoadSpecificQuestion([FromRoute] int id)
    {
        var index = id;
        if (_learningSessionCreator == null)
        {
            var ex = new Exception($"_learningSessionCreator is null. Call stack: {Environment.StackTrace}");
            ErrorLogging.Log(ex);
            throw ex;
        }

        var data = _learningSessionCreator.GetStep(index);
        return new LearningSessionResponse
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

    public record struct StepResult(AnswerState State, int Id, int Index, bool IsLastStep);

    [HttpPost]
    public StepResult? SkipStep([FromRoute] int id)
    {
        var learningSession = _learningSessionCache.GetLearningSession();
        var index = id;
        if (learningSession?.CurrentIndex == index)
        {
            learningSession.SkipStep();
            return new StepResult
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
    public StepResult[] LoadSteps()
    {
        var learningSession = _learningSessionCache.GetLearningSession(log: false);
        var result = learningSession?.Steps.Select((s, index) => new StepResult
        {
            Id = s.Question.Id,
            State = s.AnswerState,
            Index = index,
            IsLastStep = learningSession.Steps.Last() == s
        }).ToArray();

        return result;
    }
}

public static class LearningSessionConfigMapping
{
    public static LearningSessionConfig ToEntity(this LearningSessionStoreController.LearningSessionConfigRequest _request)
    {
        if (_request is null)
            throw new ArgumentNullException(nameof(_request));

        return new LearningSessionConfig
        {
            PageId = _request.PageId,
            MaxQuestionCount = _request.MaxQuestionCount,
            CurrentUserId = _request.CurrentUserId,
            IsInTestMode = _request.IsInTestMode,
            QuestionOrder = _request.QuestionOrder,
            AnswerHelp = _request.AnswerHelp,
            Repetition = _request.Repetition,
            InWishKnowledge = _request.InWishKnowledge,
            NotWishKnowledge = _request.NotInWishKnowledge,
            CreatedByCurrentUser = _request.CreatedByCurrentUser,
            NotCreatedByCurrentUser = _request.NotCreatedByCurrentUser,
            PrivateQuestions = _request.PrivateQuestions,
            PublicQuestions = _request.PublicQuestions,
            NotLearned = _request.NotLearned,
            NeedsLearning = _request.NeedsLearning,
            NeedsConsolidation = _request.NeedsConsolidation,
            Solid = _request.Solid
        };
    }
}
