public class LearningSessionResultService(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck) : IRegisterAsInstancePerLifetime
{
    public LearningSessionResultStep FillLearningSessionResult(
        LearningSession learningSession,
        LearningSessionResultStep resultStep)
    {
        var currentStep = new Step
        {
            state = learningSession.CurrentStep.AnswerState,
            id = learningSession.CurrentStep.Question.Id,
            index = learningSession.CurrentIndex,
            isLastStep = learningSession.TestIsLastStep()
        };

        resultStep.Steps = learningSession.Steps.Select((s, index) => new Step
        {
            id = s.Question.Id,
            state = s.AnswerState,
            index = index,
            isLastStep = learningSession.Steps.Last() == s
        }).ToArray();

        resultStep.ActiveQuestionCount = learningSession.Steps.DistinctBy(s => s.Question).Count();
        resultStep.CurrentStep = currentStep;
        resultStep.AnswerHelp = learningSession.Config.AnswerHelp;
        resultStep.IsInTestMode = learningSession.Config.IsInTestMode;

        return resultStep;
    }

    public (bool isValid, string? messageKey) ValidateQuestionAccess(
        int pageId,
        int questionId)
    {
        // Check if user can view the question
        if (!_permissionCheck.CanViewQuestion(questionId))
        {
            return (false, FrontendMessageKeys.Info.Question.IsPrivate);
        }

        // Check if question exists in the specified page
        var page = EntityCache.GetPage(pageId);
        var allQuestions = page.GetAggregatedQuestions(_sessionUser.UserId, permissionCheck: _permissionCheck);
        allQuestions = allQuestions.Where(q => q.Id > 0 && _permissionCheck.CanView(q)).ToList();

        bool questionNotInPage = allQuestions.All(q => q.Id != questionId);
        if (questionNotInPage)
        {
            return (false, FrontendMessageKeys.Info.Question.NotInPage);
        }

        return (true, null);
    }
}
