public class CommentAddController(
    SessionUser _sessionUser,
    CommentRepository _commentRepository,
    UserReadingRepo _userReadingRepo,
    IHttpContextAccessor _httpContextAccessor,
    QuestionChangeRepo _questionChangeRepo) : ApiBaseController
{
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public bool SaveComment([FromBody] AddCommentRequest request)
    {
        SaveComment(CommentType.AnswerQuestion, request, _sessionUser.UserId);
        return true;
    }

    private void SaveComment(CommentType type, AddCommentRequest request, int userId)
    {
        var comment = new Comment();
        comment.Type = type;
        comment.TypeId = request.id;
        comment.Text = request.text;
        comment.Title = request.title;
        comment.Creator = _userReadingRepo.GetById(userId);

        _commentRepository.Create(comment);

        var question = EntityCache.GetQuestion(request.id);
        question.AddComment(comment);
        var commentIds = question.CommentIds.ToArray();
        _questionChangeRepo.AddCommentEntry(request.id, userId, commentIds);
    }

    public record struct SaveAnswerResult(
        int Id,
        string CreatorName,
        string CreationDate,
        string CreationDateNiceText,
        string CreatorImgUrl,
        string Title,
        string Text,
        bool ShouldBeImproved,
        bool ShouldBeDeleted,
        List<string> ShouldReasons,
        bool IsSettled,
        List<CommentModel> Answers,
        int AnswersSettledCount,
        bool ShowSettledAnswers,
        string CreatorUrl);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public SaveAnswerResult? SaveAnswer([FromBody] AddAnswerType addAnswerType)
    {
        var parentComment = _commentRepository.GetById(addAnswerType.commentId);

        if (parentComment.IsSettled)
        {
            return null;
        }

        var comment = new Comment();
        comment.Type = CommentType.AnswerQuestion;
        comment.TypeId = parentComment.TypeId;
        comment.AnswerTo = parentComment;
        comment.Text = addAnswerType.text;
        comment.Creator = _userReadingRepo.GetById(_sessionUser.UserId);

        _commentRepository.Create(comment);
        var commentModel = new CommentModel(comment, _httpContextAccessor);

        var question = EntityCache.GetQuestion(comment.TypeId);
        question.AddComment(comment);
        var commentIds = question.CommentIds.ToArray();
        _questionChangeRepo.AddCommentEntry(comment.TypeId, _sessionUser.UserId, commentIds);

        return SetAnswerResult(commentModel);

    }

    public record struct MarkSettled(int commentId);
    [HttpPost]
    public bool MarkCommentAsSettled([FromBody] MarkSettled markSettled)
    {
        _commentRepository.UpdateIsSettled(markSettled.commentId, true);
        return true;
    }

    [HttpPost]
    public void MarkCommentAsUnsettled(int commentId)
    {
        _commentRepository.UpdateIsSettled(commentId, false);
    }


    private SaveAnswerResult? SetAnswerResult(CommentModel commentModel)
    {
        return new SaveAnswerResult
        {
            Answers = commentModel.Answers.ToList(),
            AnswersSettledCount = commentModel.AnswersSettledCount,
            CreationDate = commentModel.CreationDate,
            CreationDateNiceText = commentModel.CreationDateNiceText,
            CreatorName = commentModel.CreatorName,
            CreatorUrl = commentModel.CreatorUrl,
            Id = commentModel.Id,
            IsSettled = commentModel.IsSettled,
            ShouldBeDeleted = commentModel.ShouldBeDeleted,
            ShouldBeImproved = commentModel.ShouldBeImproved,
            ShouldReasons = commentModel.ShouldReasons,
            ShowSettledAnswers = commentModel.ShowSettledAnswers,
            Text = commentModel.Text,
            Title = commentModel.Title,
            CreatorImgUrl = commentModel.CreatorImgUrl

        };
    }
}
public readonly record struct AddCommentRequest(int id, string text, string title);
public readonly record struct AddAnswerType(int commentId, string text);
