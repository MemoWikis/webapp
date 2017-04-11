using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using TrueOrFalse;

public class TestSessionController : BaseController
{

    [HttpPost]
    public void RegisterQuestionAnswered(int testSessionId, int questionId, Guid questionViewGuid, bool answeredQuestion)
    {
        _sessionUser.AnsweredQuestionIds.Add(questionId);

        var currentStep = _sessionUser.GetCurrentTestSessionStep(testSessionId);

        currentStep.QuestionViewGuid = questionViewGuid;

        if (answeredQuestion)
        {
            var answers = Sl.AnswerRepo.GetByQuestionViewGuid(questionViewGuid).Where(a => !a.IsView()).ToList();
            var answer = answers.First();

            if (answers.Count > 1)
                throw new Exception("Cannot handle multiple answers to one TestSessionStep.");

            currentStep.AnswerText = answer.AnswerText;
            currentStep.AnswerState = answer.AnsweredCorrectly() ? TestSessionStepAnswerState.AnsweredCorrect : TestSessionStepAnswerState.AnsweredWrong;
        }
        else
        {
            currentStep.AnswerState = TestSessionStepAnswerState.OnlyViewedSolution;
        }
        _sessionUser.TestSessions.Find(s => s.Id == testSessionId).CurrentStepIndex++;
    }

}
