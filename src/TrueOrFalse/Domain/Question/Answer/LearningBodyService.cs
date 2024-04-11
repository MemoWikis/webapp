﻿using System.Text.RegularExpressions;
using TrueOrFalse;
using TrueOrFalse.Web;

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

public record struct LearningResult(bool Correct,
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

public class LearningBodyService(
    LearningSessionCache _learningSessionCache,
    SessionUser _sessionUser,
    SessionUserCache _sessionUserCache,
    AnswerQuestion _answerQuestion,
    AnswerLog _answerLog) : IRegisterAsInstancePerLifetime
{

    public LearningBody? GetLearningBody(int id)
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
            RenderedQuestionTextExtended: q.TextExtended != null ? MarkdownMarkdig.ToHtml(q.TextExtended) : "",
            Description: q.Description,
            HasTopics: q.Categories.Any(),
            PrimaryTopicId: primaryTopic?.Id,
            PrimaryTopicName: primaryTopic?.Name,
            Solution: q.Solution,
            IsCreator: q.Creator.Id == _sessionUser.UserId,
            IsInWishknowledge: _sessionUser.IsLoggedIn && q.IsInWishknowledge(_sessionUser.UserId, _sessionUserCache),
            QuestionViewGuid: Guid.NewGuid(),
            IsLastStep: learningSession.Steps.Last() == step);
        return learningBody;
    }

    public LearningResult GetLearningResult(int id, Guid questionViewGuid, string answer, bool isTestMode)
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

    public bool MarkQuestionAsCorrect(int id, Guid questionViewGuid, int amountOfTries)
    {
        var result = amountOfTries == 0
            ? _answerQuestion.Run(id, _sessionUser.UserId, questionViewGuid, 1, countUnansweredAsCorrect: true)
            : _answerQuestion.Run(id, _sessionUser.UserId, questionViewGuid, amountOfTries, true);

        return result != null;
    }

    public void CountLastAnswerAsCorrect(int id, Guid questionViewGuid, int interactionNumber, int? testSessionId,
        int? learningSessionId, string learningSessionStepGuid) =>
        _answerQuestion.Run(id, _sessionUser.UserId, questionViewGuid, interactionNumber, testSessionId,
            learningSessionId, learningSessionStepGuid, countLastAnswerAsCorrect: true);

    public void CountUnansweredAsCorrect(int id, Guid questionViewGuid, int interactionNumber,
        int millisecondsSinceQuestionView, string learningSessionStepGuid, int? testSessionId,
        int? learningSessionId) =>
        _answerQuestion.Run(id, _sessionUser.UserId, questionViewGuid, interactionNumber, testSessionId,
            learningSessionId, learningSessionStepGuid, millisecondsSinceQuestionView, countUnansweredAsCorrect: true);

    public SolutionResult GetSolutionResult(int id,
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
            AnswerDescription: question.Description != null ? MarkdownMarkdig.ToHtml(question.Description) : "",
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
                reference.ReferenceText = reference.ReferenceText.Replace("\n", "<br/>").Replace("\\n", "<br/>");
            if (reference.AdditionalInfo != null)
                reference.AdditionalInfo = reference.AdditionalInfo.Replace("\n", "<br/>").Replace("\\n", "<br/>");
        }
    }
}