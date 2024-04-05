using Microsoft.AspNetCore.Mvc;

namespace VueApp;
public class QuestionEditDeleteController : BaseController
{
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly QuestionDelete _questionDelete;
    private readonly LearningSessionCache _learningSessionCache;

    public QuestionEditDeleteController(QuestionReadingRepo questionReadingRepo,
        QuestionDelete questionDelete,
        LearningSessionCache learningSessionCache, SessionUser sessionUser) : base(sessionUser)
    {
        _questionReadingRepo = questionReadingRepo;
        _questionDelete = questionDelete;
        _learningSessionCache = learningSessionCache;
    }

    public record struct DeleteDetailsJson(
        string QuestionTitle,
        int TotalAnswers,
        bool CanNotBeDeleted,
        int WuwiCount,
        bool HasRights);

    [HttpGet]
    public JsonResult DeleteDetails([FromRoute] int id)
    {
        var question = _questionReadingRepo.GetById(id);
        var canBeDeleted = _questionDelete.CanBeDeleted(question.Creator.Id, question);

        return Json(new DeleteDetailsJson(

            QuestionTitle: question.Text.TruncateAtWord(90),
            TotalAnswers: question.TotalAnswers(),
            CanNotBeDeleted: !canBeDeleted.Yes,
            WuwiCount: canBeDeleted.WuwiCount,
            HasRights: canBeDeleted.HasRights
        ));
    }

    public record struct DeleteJson(
        bool ReloadAnswerBody,
    int SessionIndex,
    int Id);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult Delete([FromRoute] int id)
    {
        var updatedLearningSessionResult = _learningSessionCache.RemoveQuestionFromLearningSession(id);

        _questionDelete.Run(id);

        return Json(new DeleteJson(
            ReloadAnswerBody: updatedLearningSessionResult.reloadAnswerBody,
            SessionIndex: updatedLearningSessionResult.sessionIndex,
            id = id
        ));
    }
}
