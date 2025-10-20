using System.Text.RegularExpressions;

public class AnswerBodyController(
    LearningSessionCache _learningSessionCache,
    SessionUser _sessionUser,
    ExtendedUserCache _extendedUserCache,
    AnswerQuestion _answerQuestion,
    AnswerLog _answerLog,
    SaveQuestionView _saveQuestionView) : ApiBaseController
{
    [HttpGet]
    public LearningBody? Get([FromRoute] int id)
    {
        var answerBody = GetLearningBody(id);
        return answerBody;
    }

    public readonly record struct SendAnswerToLearningSessionRequest(
        int Id,
        Guid QuestionViewGuid,
        string Answer,
        bool InTestMode);

    [HttpPost]
    public LearningResult SendAnswerToLearningSession([FromBody] SendAnswerToLearningSessionRequest request)
    {
        return GetLearningResult(
            request.Id,
            request.QuestionViewGuid,
            request.Answer);
    }

    public readonly record struct MarkAsCorrectParam(
        int Id,
        Guid QuestionViewGuid,
        int AmountOfTries);

    [HttpPost]
    public bool MarkAsCorrect([FromBody] MarkAsCorrectParam param)
    {
        var result = param.AmountOfTries == 0
            ? _answerQuestion.SetCurrentStepAsCorrect(param.Id, _sessionUser.UserId, param.QuestionViewGuid,
                countUnansweredAsCorrect: true)
            : _answerQuestion.SetCurrentStepAsCorrect(param.Id, _sessionUser.UserId, param.QuestionViewGuid,
                countLastAnswerAsCorrect: true);

        return result != null;
    }

    public readonly record struct GetSolutionJson(
        int Id,
        Guid QuestionViewGuid,
        int InteractionNumber,
        int MillisecondsSinceQuestionView,
        bool Unanswered);

    [HttpPost]
    public SolutionResult GetSolution([FromBody] GetSolutionJson g)
    {
        return GetSolutionResult(g.Id, g.QuestionViewGuid, g.InteractionNumber,
            g.MillisecondsSinceQuestionView, g.Unanswered);
    }

    private LearningBody? GetLearningBody(int id)
    {
        var index = id;
        var learningSession = _learningSessionCache.GetLearningSession(log: false);
        if (learningSession == null || learningSession.Steps.Count == 0)
            return null;
        var step = learningSession.Steps[index];

        var question = step.Question;

        _saveQuestionView.Run(question, _sessionUser.UserId);

        var primaryPage = question.Pages.LastOrDefault();
        var title = SafeQuestionTitle.Get(question.Text);
        var learningBody = new LearningBody(
            Id: question.Id,
            Text: question.Text,
            TextHtml: question.TextHtml,
            Title: title,
            SolutionType: question.SolutionType,
            RenderedQuestionTextExtended: question.GetRenderedQuestionTextExtended(),
            Description: question.Description,
            HasPages: question.Pages.Any(),
            PrimaryPageId: primaryPage?.Id,
            PrimaryPageName: primaryPage?.Name,
            Solution: question.Solution,
            IsCreator: question.Creator.Id == _sessionUser.UserId,
            IsInWishKnowledge: _sessionUser.IsLoggedIn &&
                               question.IsInWishKnowledge(_sessionUser.UserId, _extendedUserCache),
            QuestionViewGuid: Guid.NewGuid(),
            IsLastStep: learningSession.Steps.Last() == step,
            IsPrivate: question.IsPrivate());
        return learningBody;
    }

    private LearningResult GetLearningResult(
        int id,
        Guid questionViewGuid,
        string answer)
    {
        var learningSession = _learningSessionCache.GetLearningSession();
        if (learningSession == null || learningSession.CurrentStep == null)
        {
            throw new Exception(FrontendMessageKeys.Error.Default);
        }

        learningSession.CurrentStep.Answer = answer;

        var result = _answerQuestion.Run(
            id,
            answer,
            _sessionUser.UserId,
            questionViewGuid,
            0,
            0);
        var question = EntityCache.GetQuestion(id);
        var solution = GetQuestionSolution.Run(question);

        return new LearningResult(
            Correct: result.IsCorrect,
            CorrectAnswer: result.CorrectAnswer,
            Choices: solution.GetType() == typeof(QuestionSolutionMultipleChoice_SingleSolution)
                ? ((QuestionSolutionMultipleChoice_SingleSolution)solution).Choices
                : null,
            NewStepAdded: result.NewStepAdded,
            IsLastStep: learningSession.TestIsLastStep()
        );
    }

    private SolutionResult GetSolutionResult(
        int id,
        Guid questionViewGuid,
        int interactionNumber,
        int millisecondsSinceQuestionView,
        bool unanswered)
    {
        var question = EntityCache.GetQuestion(id);
        var solution = GetQuestionSolution.Run(question);
        if (!unanswered)
            _answerLog.LogAnswerView(question, _sessionUser.UserId,
                questionViewGuid,
                interactionNumber,
                millisecondsSinceQuestionView > 0 ? millisecondsSinceQuestionView : -1);

        EscapeReferencesText(question.References);

        return new SolutionResult(
            AnswerAsHTML: solution.GetCorrectAnswerAsHtml(),
            Answer: solution.CorrectAnswer(),
            AnswerDescription: question.Description != null
                ? MarkdownMarkdig.ToHtml(question.Description)
                : "",
            AnswerDescriptionHtml: question.DescriptionHtml,
            AnswerReferences: question.References.Select(r => new AnswerReferences(
                ReferenceId: r.Id,
                PageId: r.Page?.Id ?? null,
                ReferenceType: r.ReferenceType.GetName(),
                AdditionalInfo: r.AdditionalInfo ?? "",
                ReferenceText: r.ReferenceText ?? ""
            )).ToArray());
    }

    private static void EscapeReferencesText(IList<ReferenceCacheItem> references)
    {
        foreach (var reference in references)
        {
            if (reference.ReferenceText != null)
                reference.ReferenceText = reference.ReferenceText.Replace("\n", "<br/>")
                    .Replace("\\n", "<br/>");
            if (reference.AdditionalInfo != null)
                reference.AdditionalInfo = reference.AdditionalInfo.Replace("\n", "<br/>")
                    .Replace("\\n", "<br/>");
        }
    }

    public record struct LearningBody(
        int Id,
        string Text,
        string TextHtml,
        string Title,
        SolutionType SolutionType,
        string RenderedQuestionTextExtended,
        string Description,
        bool HasPages,
        int? PrimaryPageId,
        string? PrimaryPageName,
        string Solution,
        bool IsCreator,
        bool IsInWishKnowledge,
        Guid QuestionViewGuid,
        bool IsLastStep,
        bool IsPrivate);

    public record struct LearningResult(
        bool Correct,
        string CorrectAnswer,
        List<string> Choices,
        bool NewStepAdded,
        bool IsLastStep);

    public record struct SolutionResult(
        string AnswerAsHTML,
        string Answer,
        string AnswerDescription,
        string AnswerDescriptionHtml,
        AnswerReferences[] AnswerReferences);

    public record struct AnswerReferences(
        int ReferenceId,
        int? PageId,
        string ReferenceType,
        string AdditionalInfo,
        string ReferenceText);
}