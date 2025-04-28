public class PublishQuestionStoreController(
    PermissionCheck _permissionCheck,
    QuestionWritingRepo _questionWritingRepo) : ApiBaseController
{
    [HttpGet]
    public GetShortTextResponse GetShortText([FromRoute] int id)
    {
        var question = EntityCache.GetQuestion(id);
        if (question == null)
            return new GetShortTextResponse(false, FrontendMessageKeys.Error.Default);

        if (!_permissionCheck.CanViewQuestion(id))
            return new GetShortTextResponse(false, FrontendMessageKeys.Error.Question.Rights);

        return new GetShortTextResponse(true, Text: question.GetShortTitle(96));
    }
    public record struct GetShortTextResponse(bool Success, string? MessageKey = null, string Text = "");


    [HttpPost]
    public PublishQuestionResponse PublishQuestion([FromRoute] int id)
    {
        var question = EntityCache.GetQuestion(id);

        if (question == null)
            return new PublishQuestionResponse(false, FrontendMessageKeys.Error.Default);

        if (!_permissionCheck.CanEditQuestion(id))
            return new PublishQuestionResponse(false, FrontendMessageKeys.Error.Question.Rights);

        question.Visibility = QuestionVisibility.Public;
        EntityCache.AddOrUpdate(question);
        _questionWritingRepo.Update(question);

        return new PublishQuestionResponse(true);
    }

    public record struct PublishQuestionResponse(bool Success, string? MessageKey = null);
}