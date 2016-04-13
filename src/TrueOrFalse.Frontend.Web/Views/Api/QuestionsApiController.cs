using System;
using System.Web.Mvc;

public class QuestionsApiController : BaseController
{
    [HttpPost]
    public void Pin(string questionId)
    {
        QuestionInKnowledge.Pin(Convert.ToInt32(questionId), _sessionUser.User);
    }

    [HttpPost]
    public void Unpin(string questionId)
    {
        QuestionInKnowledge.Unpin(Convert.ToInt32(questionId), _sessionUser.User);
    }
}