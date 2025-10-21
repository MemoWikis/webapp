public class QuestionEditDeleteController(
    QuestionReadingRepo _questionReadingRepo,
    QuestionDelete _questionDelete,
    LearningSessionCache _learningSessionCache)
    : ApiBaseController
{
    public record struct DeleteDetailsJson(
        string QuestionTitle,
        int TotalAnswers,
        bool CanNotBeDeleted,
        int WishKnowledgeCount,
        bool HasRights);

    [HttpGet]
    public DeleteDetailsJson DeleteDetails([FromRoute] int id)
    {
        var question = _questionReadingRepo.GetById(id);
        var canBeDeleted = _questionDelete.CanBeDeleted(question.Creator.Id, question);

        return new DeleteDetailsJson(

            QuestionTitle: question.Text.TruncateAtWord(90),
            TotalAnswers: question.TotalAnswers(),
            CanNotBeDeleted: !canBeDeleted.Yes,
            WishKnowledgeCount: canBeDeleted.WishKnowledgeCount,
            HasRights: canBeDeleted.HasRights
        );
    }

    public record struct DeleteJson(
        bool ReloadAnswerBody,
    int SessionIndex,
    int Id);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public DeleteJson Delete([FromRoute] int id)
    {
        var updatedLearningSessionResult = _learningSessionCache.RemoveQuestionFromLearningSession(id);

        _questionDelete.Run(id);

        return new DeleteJson(
            ReloadAnswerBody: updatedLearningSessionResult.reloadAnswerBody,
            SessionIndex: updatedLearningSessionResult.sessionIndex,
            Id: id
        );
    }
}
