using System;
using System.Web.Mvc;

[SetMenu(MenuEntry.None)]
public class WidgetController : BaseController
{
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

    public ActionResult Set(int setId)
    {
        return View(
            "~/Views/Widgets/WidgetSet.aspx",
            new WidgetSetModel());
    }
}