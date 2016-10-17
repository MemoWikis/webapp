using System;

public class TestSessionController : BaseController
{
    public void RegisterQuestionAnswered(int questionId, Guid questionViewGuid)
    {
        _sessionUser.AnsweredQuestionIds.Add(questionId);
        _sessionUser.TestSession.AnsweredQuestionsQuestionViewGuid.Add(questionViewGuid);
        _sessionUser.TestSession.CurrentStep++;
    }
  
}
