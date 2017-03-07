using System;
using System.Web.Mvc;

public class WidgetController : BaseController
{
    [SetMenu(MenuEntry.None)]
    public ActionResult Question(int questionId)
    {
        var answerQuestionModel = new AnswerQuestionModel(
            Guid.NewGuid(),
            R<QuestionRepo>().GetById(questionId),
            new QuestionSearchSpec());

        answerQuestionModel.DisableCommentLink = true;

        return View(
            "~/Views/Widgets/WidgetQuestion.aspx",
            new WidgetQuestionModel(answerQuestionModel));
    }
}