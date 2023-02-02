using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using TrueOrFalse;
using TrueOrFalse.Web;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class AnswerBodyController: BaseController
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
        var primaryCategory = q.Categories.LastOrDefault();
        var model = new
        {
            id = q.Id,
            text = q.Text,
            title = Regex.Replace(q.Text, "<.*?>", String.Empty),
            solutionType = q.SolutionType,
            renderedQuestionTextExtended = q.TextExtended != null ? MarkdownMarkdig.ToHtml(q.TextExtended) : "",
            description = q.Description,
            hasTopics = q.Categories.Any(),
            primaryTopicUrl = primaryCategory?.Url,
            primaryTopicName = primaryCategory?.Name,
            solution = q.Solution,

            isCreator = q.Creator.Id = SessionUser.UserId,
            isInWishKnowledge = SessionUser.IsLoggedIn && q.IsInWishknowledge(),

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
            isLastStep = learningSession.IsLastStep
        });
    }

    [HttpPost]
    public JsonResult MarkAsCorrect(int id, Guid questionViewGuid, int amountOfTries)
    {
        var result = amountOfTries == 0 ? 
            _answerQuestion.Run(id, SessionUser.UserId, questionViewGuid, 1, countUnansweredAsCorrect: true): 
            _answerQuestion.Run(id, SessionUser.UserId, questionViewGuid, amountOfTries, true);
        if (result != null)
        {
            return Json(true);
        }
        return Json(false);
    }


    [HttpPost]
    public void CountLastAnswerAsCorrect(int id, Guid questionViewGuid, int interactionNumber, int? testSessionId, int? learningSessionId, string learningSessionStepGuid) =>
        _answerQuestion.Run(id, SessionUser.UserId, questionViewGuid, interactionNumber, testSessionId, learningSessionId, learningSessionStepGuid, countLastAnswerAsCorrect: true);

    [HttpPost]
    public void CountUnansweredAsCorrect(int id, Guid questionViewGuid, int interactionNumber, int millisecondsSinceQuestionView, string learningSessionStepGuid, int? testSessionId, int? learningSessionId) =>
        _answerQuestion.Run(id, SessionUser.UserId, questionViewGuid, interactionNumber, testSessionId, learningSessionId, learningSessionStepGuid, millisecondsSinceQuestionView, countUnansweredAsCorrect: true);

}