using System.Web.Mvc;

namespace VueApp;
public class QuestionEditDeleteController : BaseController
{
    private readonly QuestionRepo _questionRepo;

    public QuestionEditDeleteController(QuestionRepo questionRepo)
    {
        _questionRepo = questionRepo;
    }

    [HttpGet]
    public JsonResult DeleteDetails(int questionId)
    {
        var question = _questionRepo.GetById(questionId);
        var canBeDeleted = QuestionDelete.CanBeDeleted(question.Creator.Id, question);

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
        var updatedLearningSessionResult = LearningSessionCache.RemoveQuestionFromLearningSession(questionId);

        QuestionDelete.Run(questionId);
        return Json(new
        {
            reloadAnswerBody = updatedLearningSessionResult.reloadAnswerBody,
            sessionIndex = updatedLearningSessionResult.sessionIndex,
            id = questionId
        });
    }
}