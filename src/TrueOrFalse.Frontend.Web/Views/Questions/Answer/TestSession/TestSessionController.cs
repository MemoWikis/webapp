using System;

public class TestSessionController : BaseController
{
    public void RegisterQuestionAnswered(int questionId, Guid questionViewGuid)
    {
        //_sessionUser.AnsweredQuestionIds.Add(questionId); //doesn't work

        //alternative
        var answeredQuestionIds = _sessionUser.AnsweredQuestionIds;
        answeredQuestionIds.Add(questionId); //works
        _sessionUser.AnsweredQuestionIds = answeredQuestionIds; //doesn't work

        _sessionUser.TestSession.AnsweredQuestionsQuestionViewGuid.Add(questionViewGuid);
        _sessionUser.TestSession.CurrentStep++;
    }
  
}
