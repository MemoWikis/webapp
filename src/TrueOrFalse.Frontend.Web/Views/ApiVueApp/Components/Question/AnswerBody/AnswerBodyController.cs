using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using TrueOrFalse.Web;
using AnswerBodyHelper;


public class AnswerBodyController : Controller {
    private readonly AnswerQuestion _answerQuestion;
    private readonly SessionUser _sessionUser;
    private readonly LearningSessionCache _learningSessionCache;
    private readonly AnswerLog _answerLog;
    private readonly SessionUserCache _sessionUserCache;

    public AnswerBodyController(AnswerQuestion answerQuestion,
        SessionUser sessionUser,
        LearningSessionCache learningSessionCache,
        AnswerLog answerLog,
        SessionUserCache sessionUserCache)
    {
        _answerQuestion = answerQuestion;
        _sessionUser = sessionUser;
        _learningSessionCache = learningSessionCache;
        _answerLog = answerLog;
        _sessionUserCache = sessionUserCache;
    }

    [HttpGet]
    public JsonResult Get([FromRoute] int index)
    {
        var learningSession = _learningSessionCache.GetLearningSession();
        if (learningSession.Steps.Count == 0)
            return Json(null);
        var step = learningSession.Steps[index];

        var q = step.Question;
        var primaryTopic = q.Categories.LastOrDefault();
        var title = Regex.Replace(q.Text, "<.*?>", String.Empty);
        var model = new
        {
            id = q.Id,
            text = q.Text,
            title = title,
            solutionType = q.SolutionType,
            renderedQuestionTextExtended = q.TextExtended != null ? MarkdownMarkdig.ToHtml(q.TextExtended) : "",
            description = q.Description,
            hasTopics = q.Categories.Any(),
            primaryTopicId = primaryTopic?.Id,
            primaryTopicName = primaryTopic?.Name,
            solution = q.Solution,

            isCreator = q.Creator.Id == _sessionUser.UserId,
            isInWishknowledge = _sessionUser.IsLoggedIn && q.IsInWishknowledge(_sessionUser.UserId, _sessionUserCache),

            questionViewGuid = Guid.NewGuid(),
            isLastStep = learningSession.Steps.Last() == step
        };
        return Json(model);
    }

    public readonly record struct SendAnswerToLearningSessionJson(int Id, Guid QuestionViewGuid, string Answer, bool InTestMode);

    [HttpPost]
    public JsonResult SendAnswerToLearningSession([FromBody] SendAnswerToLearningSessionJson sendAnswerToLearningSession)
    {
        var answer = sendAnswerToLearningSession.Answer;
        var id = sendAnswerToLearningSession.Id;

        var learningSession = _learningSessionCache.GetLearningSession();
        learningSession.CurrentStep.Answer = answer;

        var result = _answerQuestion.Run(id, answer, _sessionUser.UserId, sendAnswerToLearningSession.QuestionViewGuid, 0,
            0, 0, new Guid(), sendAnswerToLearningSession.InTestMode);
        var question = EntityCache.GetQuestion(id);
        var solution = GetQuestionSolution.Run(question);

        return Json(new
        {
            correct = result.IsCorrect,
            correctAnswer = result.CorrectAnswer,
            choices = solution.GetType() == typeof(QuestionSolutionMultipleChoice_SingleSolution)
                ? ((QuestionSolutionMultipleChoice_SingleSolution)solution).Choices
                : null,
            newStepAdded = result.NewStepAdded,
            isLastStep = learningSession.TestIsLastStep()
        });
    }

    public readonly record struct MarkAsCorrectJson(int Id, Guid QuestionViewGuid, int AmountOfTries);
    [HttpPost]
    public bool MarkAsCorrect([FromBody] MarkAsCorrectJson markAsCorrectData)
    {
        var id = markAsCorrectData.Id;
        var questionViewGuid = markAsCorrectData.QuestionViewGuid;

        var result = markAsCorrectData.AmountOfTries == 0
            ? _answerQuestion.Run(id, _sessionUser.UserId, questionViewGuid, 1, countUnansweredAsCorrect: true)
            : _answerQuestion.Run(id, _sessionUser.UserId, questionViewGuid, markAsCorrectData.AmountOfTries, true);

        return result != null;
    }


    [HttpPost]
    public void CountLastAnswerAsCorrect(int id, Guid questionViewGuid, int interactionNumber, int? testSessionId,
        int? learningSessionId, string learningSessionStepGuid) =>
        _answerQuestion.Run(id, _sessionUser.UserId, questionViewGuid, interactionNumber, testSessionId,
            learningSessionId, learningSessionStepGuid, countLastAnswerAsCorrect: true);

    [HttpPost]
    public void CountUnansweredAsCorrect(int id, Guid questionViewGuid, int interactionNumber,
        int millisecondsSinceQuestionView, string learningSessionStepGuid, int? testSessionId,
        int? learningSessionId) =>
        _answerQuestion.Run(id, _sessionUser.UserId, questionViewGuid, interactionNumber, testSessionId,
            learningSessionId, learningSessionStepGuid, millisecondsSinceQuestionView, countUnansweredAsCorrect: true);

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

    public readonly record struct GetSolutionJson(int Id, Guid QuestionViewGuid, int InteractionNumber, int MillisecondsSinceQuestionView, bool Unanswered);

    [HttpPost]
    public JsonResult GetSolution([FromBody] GetSolutionJson getSolutionData)
    {
        var question = EntityCache.GetQuestion(getSolutionData.Id);
        var solution = GetQuestionSolution.Run(question);
        if (!getSolutionData.Unanswered)
            _answerLog.LogAnswerView(question, _sessionUser.UserId,
                getSolutionData.QuestionViewGuid, 
                getSolutionData.InteractionNumber,
                getSolutionData.MillisecondsSinceQuestionView > 0 ? getSolutionData.MillisecondsSinceQuestionView : -1);

        EscapeReferencesText(question.References);

        return Json(new
            {
                answerAsHTML = solution.GetCorrectAnswerAsHtml(),
                answer = solution.CorrectAnswer(),
                answerDescription = question.Description != null ? MarkdownMarkdig.ToHtml(question.Description) : "",
                answerReferences = question.References.Select(r => new
                {
                    referenceId = r.Id,
                    topicId = r.Category?.Id ?? null,
                    referenceType = r.ReferenceType.GetName(),
                    additionalInfo = r.AdditionalInfo ?? "",
                    referenceText = r.ReferenceText ?? ""
                }).ToArray()
            }
        );
    }
}
