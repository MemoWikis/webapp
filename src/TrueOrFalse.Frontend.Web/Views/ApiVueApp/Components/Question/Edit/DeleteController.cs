using Microsoft.AspNetCore.Mvc;

namespace VueApp;
public class QuestionEditDeleteController : Controller
{
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly QuestionDelete _questionDelete;
    private readonly LearningSessionCache _learningSessionCache;

    public QuestionEditDeleteController(QuestionReadingRepo questionReadingRepo,
        QuestionDelete questionDelete,
        LearningSessionCache learningSessionCache)
    {
        _questionReadingRepo = questionReadingRepo;
        _questionDelete = questionDelete;
        _learningSessionCache = learningSessionCache;
    }

    [HttpGet]
    public JsonResult DeleteDetails([FromRoute] int id)
    {
        var question = _questionReadingRepo.GetById(id);
        var canBeDeleted = _questionDelete.CanBeDeleted(question.Creator.Id, question);

        return Json(new
        {
            questionTitle = question.Text.TruncateAtWord(90),
            totalAnswers = question.TotalAnswers(),
            canNotBeDeleted = !canBeDeleted.Yes,
            wuwiCount = canBeDeleted.WuwiCount,
            hasRights = canBeDeleted.HasRights
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult Delete([FromRoute] int id)
    {
        var updatedLearningSessionResult = _learningSessionCache.RemoveQuestionFromLearningSession(id);

        _questionDelete.Run(id);
        return Json(new
        {
            reloadAnswerBody = updatedLearningSessionResult.reloadAnswerBody,
            sessionIndex = updatedLearningSessionResult.sessionIndex,
            id = id
        });
    }
}
    