using System;
using System.Linq;

public class TestSessionController : BaseController
{
    public void RegisterQuestionAnswered(int questionId, Guid questionViewGuid, bool answeredQuestion)
    {
        _sessionUser.AnsweredQuestionIds.Add(questionId);

        _sessionUser.TestSession.Steps.ElementAt(_sessionUser.TestSession.CurrentStep-1).QuestionViewGuid = questionViewGuid;
        if (answeredQuestion)
        {
            var answers = Sl.R<AnswerRepo>().GetByQuestionViewGuid(questionViewGuid).Where(a => !a.IsView()).ToList();
            if (answers.Count > 1)
                throw new Exception("Cannot handle multiple answers to one TestSessionStep.");
            _sessionUser.TestSession.Steps.ElementAt(_sessionUser.TestSession.CurrentStep-1).AnswerText = answers.FirstOrDefault().AnswerText;
            _sessionUser.TestSession.Steps.ElementAt(_sessionUser.TestSession.CurrentStep-1).AnswerState = 
                answers.FirstOrDefault().AnsweredCorrectly() ? TestSessionStepAnswerState.AnsweredCorrect : TestSessionStepAnswerState.AnsweredWrong;
        }
        else
        {
            _sessionUser.TestSession.Steps.ElementAt(_sessionUser.TestSession.CurrentStep-1).AnswerState = TestSessionStepAnswerState.OnlyViewedSolution;
        }
        _sessionUser.TestSession.CurrentStep++;
    }
  
}
