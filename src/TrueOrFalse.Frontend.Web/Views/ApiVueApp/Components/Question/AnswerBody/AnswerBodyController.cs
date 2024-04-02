using System;
using Microsoft.AspNetCore.Mvc;

public class AnswerBodyController(LearningBodyService _learningBodyService) : Controller

{
    [HttpGet]
    public LearningBody? Get([FromRoute] int id)
    {
       return _learningBodyService.GetLearningBody(id);
    }

    public readonly record struct SendAnswerToLearningSessionJson(int Id, Guid QuestionViewGuid, string Answer, bool InTestMode);

    [HttpPost]
    public LearningResult SendAnswerToLearningSession([FromBody] SendAnswerToLearningSessionJson sendAnswerToLearningSession)
    {
        return _learningBodyService.GetLearningResult(sendAnswerToLearningSession.Id,
            sendAnswerToLearningSession.QuestionViewGuid,
            sendAnswerToLearningSession.Answer,
            sendAnswerToLearningSession.InTestMode);
    }

    public readonly record struct MarkAsCorrectJson(int Id, Guid QuestionViewGuid, int AmountOfTries);
    [HttpPost]
    public bool MarkAsCorrect([FromBody] MarkAsCorrectJson m)
    {
        return _learningBodyService.MarkQuestionAsCorrect(m.Id, m.QuestionViewGuid, m.AmountOfTries); 
    }

    [HttpPost]
    public void CountLastAnswerAsCorrect(int id, Guid questionViewGuid, int interactionNumber, int? testSessionId,
        int? learningSessionId, string learningSessionStepGuid) =>
        _learningBodyService.CountLastAnswerAsCorrect(id, questionViewGuid, interactionNumber, testSessionId,
            learningSessionId, learningSessionStepGuid);

    [HttpPost]
    public void CountUnansweredAsCorrect(int id, Guid questionViewGuid, int interactionNumber,
        int millisecondsSinceQuestionView, string learningSessionStepGuid, int? testSessionId,
        int? learningSessionId) =>
        _learningBodyService.CountUnansweredAsCorrect(id, questionViewGuid, interactionNumber, millisecondsSinceQuestionView, learningSessionStepGuid, testSessionId,
            learningSessionId);

    public readonly record struct GetSolutionJson(int Id, Guid QuestionViewGuid, int InteractionNumber, int MillisecondsSinceQuestionView, bool Unanswered);

    [HttpPost]
    public SolutionResult GetSolution([FromBody] GetSolutionJson g)
    {
       return _learningBodyService.GetSolutionResult(g.Id, g.QuestionViewGuid, g.InteractionNumber,
            g.MillisecondsSinceQuestionView, g.Unanswered);
    }
}
