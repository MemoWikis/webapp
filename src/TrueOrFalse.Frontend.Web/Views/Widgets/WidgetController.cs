using System.Web.Mvc;

public class WidgetController : BaseController
{
    [SetMenu(MenuEntry.None)]
    public ActionResult Question(int questionId)
    {
        return View(
            "~/Views/Widgets/WidgetQuestion.aspx",
            new WidgetQuestionModel(
                new AnswerQuestionModel(
                    R<QuestionRepo>().GetById(questionId),
                    new QuestionSearchSpec())
            ));
    }
}