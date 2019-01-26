using System.Web.Mvc;

public class QuestionHistoryController : Controller
{
    private const string _viewLocation = "~/Views/Questions/History/QuestionHistory.aspx";

    [AccessOnlyAsQuestionOwner]
    public ActionResult List(int questionId)
    {
        var question = Sl.QuestionRepo.GetById(questionId);

        var questionChanges = Sl.QuestionChangeRepo.GetForQuestion(questionId);

        return View(_viewLocation, new QuestionHistoryModel(question, questionChanges));
    }
}
