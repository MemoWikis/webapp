using System;
using System.Linq;

public class TestSessionController : BaseController
{
    public void RegisterQuestionAnswered(int testSessionId, int questionId, Guid questionViewGuid, bool answeredQuestion)
    {
        _sessionUser.AnsweredQuestionIds.Add(questionId);
        var currentStep = _sessionUser.TestSessions.Find(s => s.Id == testSessionId).CurrentStep - 1;
        //_sessionUser.TestSessions.Find(s => s.Id == testSessionId).Steps
        _sessionUser.TestSessions.Find(s => s.Id == testSessionId).Steps.ElementAt(currentStep).QuestionViewGuid = questionViewGuid;
        if (answeredQuestion)
        {
            var answers = Sl.R<AnswerRepo>().GetByQuestionViewGuid(questionViewGuid).Where(a => !a.IsView()).ToList();
            if (answers.Count > 1)
                throw new Exception("Cannot handle multiple answers to one TestSessionStep.");
            _sessionUser.TestSessions.Find(s => s.Id == testSessionId).Steps.ElementAt(currentStep).AnswerText = answers.FirstOrDefault().AnswerText;
            _sessionUser.TestSessions.Find(s => s.Id == testSessionId).Steps.ElementAt(currentStep).AnswerState = 
                answers.FirstOrDefault().AnsweredCorrectly() ? TestSessionStepAnswerState.AnsweredCorrect : TestSessionStepAnswerState.AnsweredWrong;
        }
        else
        {
            _sessionUser.TestSessions.Find(s => s.Id == testSessionId).Steps.ElementAt(currentStep).AnswerState = TestSessionStepAnswerState.OnlyViewedSolution;
        }
        _sessionUser.TestSessions.Find(s => s.Id == testSessionId).CurrentStep++;
    }
  
}
