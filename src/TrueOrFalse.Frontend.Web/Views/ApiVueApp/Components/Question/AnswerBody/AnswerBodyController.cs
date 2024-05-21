using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using TrueOrFalse;
using TrueOrFalse.Web;

public class AnswerBodyController(
    LearningSessionCache _learningSessionCache,
    SessionUser _sessionUser,
    ExtendedUserCache _extendedUserCache,
    AnswerQuestion _answerQuestion,
    AnswerLog _answerLog) : Controller
{
    [HttpGet]
    public LearningBody? Get([FromRoute] int id)
    {
        var answerBody = GetLearningBody(id);
        return answerBody;
    }

    public readonly record struct SendAnswerToLearningSessionJson(
        int Id,
        Guid QuestionViewGuid,
        string Answer,
        bool InTestMode);

    [HttpPost]
    public LearningResult SendAnswerToLearningSession(
        [FromBody] SendAnswerToLearningSessionJson sendAnswerToLearningSession)
    {
        return GetLearningResult(sendAnswerToLearningSession.Id,
            sendAnswerToLearningSession.QuestionViewGuid,
            sendAnswerToLearningSession.Answer,
            sendAnswerToLearningSession.InTestMode);
    }

    public readonly record struct MarkAsCorrectJson(
        int Id,
        Guid QuestionViewGuid,
        int AmountOfTries);

    [HttpPost]
    public bool MarkAsCorrect([FromBody] MarkAsCorrectJson m)
    {
        var result = m.AmountOfTries == 0
            ? _answerQuestion.Run(m.Id, _sessionUser.UserId, m.QuestionViewGuid, 1,
                countUnansweredAsCorrect: true)
            : _answerQuestion.Run(m.Id, _sessionUser.UserId, m.QuestionViewGuid, m.AmountOfTries,
                true);

        return result != null;
    }

    [HttpPost]
    public void CountLastAnswerAsCorrect(
        int id,
        Guid questionViewGuid,
        int interactionNumber,
        int? testSessionId,
        int? learningSessionId,
        string learningSessionStepGuid) =>
        CountLastAnswerAsCorrect(id, questionViewGuid, interactionNumber, testSessionId,
            learningSessionId, learningSessionStepGuid);

    [HttpPost]
    public void CountUnansweredAsCorrect(
        int id,
        Guid questionViewGuid,
        int interactionNumber,
        int millisecondsSinceQuestionView,
        string learningSessionStepGuid,
        int? testSessionId,
        int? learningSessionId) =>
        CountUnansweredAsCorrect(id, questionViewGuid, interactionNumber,
            millisecondsSinceQuestionView, learningSessionStepGuid, testSessionId,
            learningSessionId);

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
        var learningSession = _learningSessionCache.GetLearningSession();
        if (learningSession == null || learningSession.Steps.Count == 0)
            return null;
        var step = learningSession.Steps[index];

        var q = step.Question;
        var primaryTopic = q.Categories.LastOrDefault();
        var title = Regex.Replace(q.Text, "<.*?>", String.Empty);
        var learningBody = new LearningBody(
            Id: q.Id,
            Text: q.Text,
            TextHtml: q.TextHtml,
            Title: title,
            SolutionType: q.SolutionType,
            RenderedQuestionTextExtended: q.TextExtended != null
                ? MarkdownMarkdig.ToHtml(q.TextExtended)
                : "",
            Description: q.Description,
            HasTopics: q.Categories.Any(),
            PrimaryTopicId: primaryTopic?.Id,
            PrimaryTopicName: primaryTopic?.Name,
            Solution: q.Solution,
            IsCreator: q.Creator.Id == _sessionUser.UserId,
            IsInWishknowledge: _sessionUser.IsLoggedIn &&
                               q.IsInWishknowledge(_sessionUser.UserId, _extendedUserCache),
            QuestionViewGuid: Guid.NewGuid(),
            IsLastStep: learningSession.Steps.Last() == step);
        return learningBody;
    }

    private LearningResult GetLearningResult(
        int id,
        Guid questionViewGuid,
        string answer,
        bool isTestMode)
    {
        var learningSession = _learningSessionCache.GetLearningSession();
        learningSession.CurrentStep.Answer = answer;

        var result = _answerQuestion.Run(id, answer, _sessionUser.UserId, questionViewGuid, 0,
            0, 0, new Guid(), isTestMode);
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
            AnswerReferences: question.References.Select(r => new AnswerReferences(
                ReferenceId: r.Id,
                TopicId: r.Category?.Id ?? null,
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
        bool HasTopics,
        int? PrimaryTopicId,
        string? PrimaryTopicName,
        string Solution,
        bool IsCreator,
        bool IsInWishknowledge,
        Guid QuestionViewGuid,
        bool IsLastStep);

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
        AnswerReferences[] AnswerReferences);

    public record struct AnswerReferences(
        int ReferenceId,
        int? TopicId,
        string ReferenceType,
        string AdditionalInfo,
        string ReferenceText);
}