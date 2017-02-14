using System;
using System.Web.Mvc;

public class SetVideoController : BaseController
{
    public string RenderAnswerBody(int questionId)
    {
        return ViewRenderer.RenderPartialView(
            "~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx",
            new AnswerBodyModel(new AnswerQuestionModel(R<QuestionRepo>().GetById(questionId))),
            ControllerContext
        );
    }

    public EmptyResult SaveTimeCode(string questionInSetId, string timeCode)
    {
        R<QuestionInSetRepo>().SaveTimecode(Convert.ToInt32(questionInSetId), timeCode);
        
        return new EmptyResult();
    }
}