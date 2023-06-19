using System.Web.Mvc;

namespace VueApp;
public class QuestionEditDeleteController : Controller
{
    private readonly QuestionRepo _questionRepo;
    private readonly QuestionDelete _questionDelete;

    public QuestionEditDeleteController(QuestionRepo questionRepo, QuestionDelete questionDelete)
    {
        _questionRepo = questionRepo;
        _questionDelete = questionDelete;
    }

    [HttpGet]
    public JsonResult DeleteDetails(int questionId)
    {
        var question = _questionRepo.GetById(questionId);
        var canBeDeleted = _questionDelete.CanBeDeleted(question.Creator.Id, question);

        return Json(new
        {
            questionTitle = question.Text.TruncateAtWord(90),
            totalAnswers = question.TotalAnswers(),
            canNotBeDeleted = !canBeDeleted.Yes,
            wuwiCount = canBeDeleted.WuwiCount,
            hasRights = canBeDeleted.HasRights
        }, JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    public JsonResult Delete(int questionId, int sessionIndex)
    {
        _questionDelete.Run(questionId);
        LearningSessionCacheLegacy.RemoveQuestionFromLearningSession(sessionIndex, questionId);
        return Json(new
        {
            sessionIndex,
            questionId
        });
    }
}