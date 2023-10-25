using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HelperClassesControllers;
using Microsoft.AspNetCore.Mvc;
using TrueOrFalse.Web;


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
    public JsonResult Get(int index)
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

    [HttpPost]
    public JsonResult SendAnswerToLearningSession([FromBody] SendAnswerToLearningSession sendAnswerToLearningSession)
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

    [HttpPost]
    public JsonResult MarkAsCorrect([FromBody] MarkAsCorrectData markAsCorrectData)
    {
        var id = markAsCorrectData.Id;
        var questionViewGuid = markAsCorrectData.QuestionViewGuid;

        var result = markAsCorrectData.AmountOfTries == 0
            ? _answerQuestion.Run(id, _sessionUser.UserId, questionViewGuid, 1, countUnansweredAsCorrect: true)
            : _answerQuestion.Run(id, _sessionUser.UserId, questionViewGuid, markAsCorrectData.AmountOfTries, true);
        if (result != null)
        {
            return Json(true);
        }

        return Json(false);
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


    [HttpPost]
    public JsonResult GetSolution([FromBody] GetSolutionData getSolutionData)
    {
        var question = EntityCache.GetQuestion(getSolutionData.Id);
        var solution = GetQuestionSolution.Run(question);
        if (!getSolutionData.Unanswered)
            _answerLog.LogAnswerView(question, _sessionUser.UserId,
                getSolutionData.QuestionViewGuid, 
                getSolutionData.InteractionNumber,
                getSolutionData.MillisecondsSinceQuestionView);

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

namespace HelperClassesControllers
{
    public class SendAnswerToLearningSession
    {
        public int Id { get; set; }
        public Guid QuestionViewGuid { get; set; }
        public string Answer { get; set; }
        public bool InTestMode { get; set; }

    }

    public class GetSolutionData
    {
        public int Id { get; set; }
        public Guid QuestionViewGuid { get; set; }
        public int InteractionNumber { get; set; }
        public int MillisecondsSinceQuestionView { get; set; } = -1;
        public bool Unanswered { get; set; } = false;
    }

    public class MarkAsCorrectData
    {
        public int Id { get; set; }
        public Guid QuestionViewGuid { get; set; }
        public int AmountOfTries { get; set; }
    }
}