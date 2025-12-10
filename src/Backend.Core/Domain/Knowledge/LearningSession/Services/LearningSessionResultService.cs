public class LearningSessionResultService(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    ExtendedUserCache _extendedUserCache) : IRegisterAsInstancePerLifetime
{
    public LearningSessionResultStep FillLearningSessionResult(
        LearningSession learningSession,
        LearningSessionResultStep resultStep)
    {
        if (learningSession?.CurrentStep == null)
        {
            return resultStep;
        }
        
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
        if (page == null)
        {
            return (false, FrontendMessageKeys.Info.Question.NotInPage);
        }
        
        var allQuestions = page.GetAggregatedQuestions(_sessionUser.UserId, permissionCheck: _permissionCheck);
        allQuestions = allQuestions.Where(q => q.Id > 0 && _permissionCheck.CanView(q)).ToList();

        bool questionNotInPage = allQuestions.All(q => q.Id != questionId);
        if (questionNotInPage)
        {
            return (false, FrontendMessageKeys.Info.Question.NotInPage);
        }

        return (true, null);
    }

    public (bool isValid, string? messageKey) ValidateWishknowledgeQuestionAccess(
        int userId,
        int questionId)
    {
        // Check if user can view the question
        if (!_permissionCheck.CanViewQuestion(questionId))
        {
            return (false, FrontendMessageKeys.Info.Question.IsPrivate);
        }

        // Check if question exists in user's wishknowledge
        var user = _extendedUserCache.GetUser(userId);
        var wishknowledgeQuestionIds = user.QuestionValuations
            .Where(questionValuation => questionValuation.Value.IsInWishKnowledge)
            .Select(questionValuation => questionValuation.Key)
            .ToList();

        bool questionNotInWishknowledge = !wishknowledgeQuestionIds.Contains(questionId);
        if (questionNotInWishknowledge)
        {
            return (false, FrontendMessageKeys.Info.Question.NotInFilter);
        }

        return (true, null);
    }
}
