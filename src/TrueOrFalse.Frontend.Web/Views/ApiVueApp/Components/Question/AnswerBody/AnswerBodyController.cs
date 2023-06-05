using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using TrueOrFalse;
using TrueOrFalse.Web;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class AnswerBodyController : BaseController
{
    private readonly QuestionRepo _questionRepo;
    private readonly AnswerQuestion _answerQuestion;

    public AnswerBodyController(QuestionRepo questionRepo, AnswerQuestion answerQuestion)
    {
        _questionRepo = questionRepo;
        _answerQuestion = answerQuestion;
    }

    [HttpGet]
    public JsonResult Get(int index)
    {
        var learningSession = LearningSessionCache.GetLearningSession();
        var step = learningSession.Steps[index];

        var q = step.Question;
        var primaryTopic = q.Categories.LastOrDefault();
        var title = Regex.Replace(q.Text, "<.*?>", String.Empty);
        var model = new
        {
            id = q.Id,
            text = q.Text,
            title = title,
            encodedTitle = UriSanitizer.Run(title, 10),
            solutionType = q.SolutionType,
            renderedQuestionTextExtended = q.TextExtended != null ? MarkdownMarkdig.ToHtml(q.TextExtended) : "",
            description = q.Description,
            hasTopics = q.Categories.Any(),
            primaryTopicUrl = "/" + UriSanitizer.Run(primaryTopic?.Name) + "/" + primaryTopic?.Id,
            primaryTopicName = primaryTopic?.Name,
            solution = q.Solution,

            isCreator = q.Creator.Id = SessionUserLegacy.UserId,
            isInWishknowledge = SessionUserLegacy.IsLoggedIn && q.IsInWishknowledge(),

            questionViewGuid = Guid.NewGuid(),
            isLastStep = learningSession.Steps.Last() == step
        };
        return Json(model, JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    public JsonResult SendAnswerToLearningSession(
        int id,
        Guid questionViewGuid = new Guid(),
        string answer = "",
        bool inTestMode = false)
    {
        var learningSession = LearningSessionCache.GetLearningSession();
        learningSession.CurrentStep.Answer = answer;

        var result = _answerQuestion.Run(id, answer, UserId, questionViewGuid, 0,
            0, 0, new Guid(), inTestMode);
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

    [HttpPost]
    public JsonResult MarkAsCorrect(int id, Guid questionViewGuid, int amountOfTries)
    {
        var result = amountOfTries == 0
            ? _answerQuestion.Run(id, SessionUserLegacy.UserId, questionViewGuid, 1, countUnansweredAsCorrect: true)
            : _answerQuestion.Run(id, SessionUserLegacy.UserId, questionViewGuid, amountOfTries, true);
        if (result != null)
        {
            return Json(true);
        }

        return Json(false);
    }


    [HttpPost]
    public void CountLastAnswerAsCorrect(int id, Guid questionViewGuid, int interactionNumber, int? testSessionId,
        int? learningSessionId, string learningSessionStepGuid) =>
        _answerQuestion.Run(id, SessionUserLegacy.UserId, questionViewGuid, interactionNumber, testSessionId,
            learningSessionId, learningSessionStepGuid, countLastAnswerAsCorrect: true);

    [HttpPost]
    public void CountUnansweredAsCorrect(int id, Guid questionViewGuid, int interactionNumber,
        int millisecondsSinceQuestionView, string learningSessionStepGuid, int? testSessionId,
        int? learningSessionId) =>
        _answerQuestion.Run(id, SessionUserLegacy.UserId, questionViewGuid, interactionNumber, testSessionId,
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


    [HttpPost]
    public JsonResult GetSolution(int id, Guid questionViewGuid, int interactionNumber,
        int millisecondsSinceQuestionView = -1, bool unanswered = false)
    {
        var question = EntityCache.GetQuestion(id);
        var solution = GetQuestionSolution.Run(question);
        if (!unanswered)
            R<AnswerLog>().LogAnswerView(question, this.UserId, questionViewGuid, interactionNumber,
                millisecondsSinceQuestionView);

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