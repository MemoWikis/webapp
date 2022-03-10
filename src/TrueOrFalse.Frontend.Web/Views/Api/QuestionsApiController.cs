using System;
using System.Web.Mvc;

public class QuestionsApiController : BaseController
{
    [HttpPost]
    public bool Pin(string questionId)
    {
        if (SessionUser.User == null)
            return false;
        QuestionInKnowledge.Pin(Convert.ToInt32(questionId), SessionUser.User);
        return true;
    }

    [HttpPost]
    public bool Unpin(string questionId)
    {
        if (SessionUser.User == null)
            return false;
        QuestionInKnowledge.Unpin(Convert.ToInt32(questionId), SessionUser.User);
        return true;
    }
}