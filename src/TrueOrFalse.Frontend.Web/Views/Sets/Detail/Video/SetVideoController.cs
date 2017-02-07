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
}