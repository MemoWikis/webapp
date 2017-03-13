using System;
using System.Web.Mvc;

public class SetVideoController : BaseController
{
    public string RenderAnswerBody(int questionId)
    {
        var answerBody = new AnswerBodyModel(new AnswerQuestionModel(R<QuestionRepo>().GetById(questionId)));
        answerBody.DisableCommentLink = true;

        return ViewRenderer.RenderPartialView(
            "~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx", 
            answerBody,
            ControllerContext
        );
    }

    public EmptyResult SaveTimeCode(string questionInSetId, string timeCode)
    {
        R<QuestionInSetRepo>().SaveTimecode(Convert.ToInt32(questionInSetId), timeCode);
        
        return new EmptyResult();
    }
}