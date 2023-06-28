using System.Web.Mvc;

namespace VueApp;
public class QuestionEditDeleteController : Controller
{
    private readonly QuestionRepo _questionRepo;
    private readonly QuestionDelete _questionDelete;
    private readonly LearningSessionCache _learningSessionCache;

    public QuestionEditDeleteController(QuestionRepo questionRepo,
        QuestionDelete questionDelete,
        LearningSessionCache learningSessionCache)
    {
        _questionRepo = questionRepo;
        _questionDelete = questionDelete;
        _learningSessionCache = learningSessionCache;
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
    public JsonResult Delete(int questionId)
    {
        var updatedLearningSessionResult = _learningSessionCache.RemoveQuestionFromLearningSession(questionId);

        _questionDelete.Run(questionId);
        return Json(new
        {
            reloadAnswerBody = updatedLearningSessionResult.reloadAnswerBody,
            sessionIndex = updatedLearningSessionResult.sessionIndex,
            id = questionId
        });
    }
}
    