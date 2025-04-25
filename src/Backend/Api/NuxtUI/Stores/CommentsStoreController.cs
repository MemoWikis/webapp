public class CommentsStoreController(
    CommentRepository _commentRepository,
    IHttpContextAccessor _httpContextAccessor,
    PermissionCheck _permissionCheck) : ApiBaseController
{
    [HttpGet]
    public Comments GetAllComments([FromRoute] int id)
    {
        var comments = _commentRepository.GetForDisplay(id);

        var settledComments = comments.Where(c => c.IsSettled).Select(c => GetComment(c)).ToArray();
        var unsettledComments = comments.Where(c => !c.IsSettled).Select(c => GetComment(c)).ToArray();
        var result = new Comments(
            SettledComments: settledComments,
            UnsettledComments: unsettledComments);
        return result;
    }

    [HttpGet]
    public CommentJson GetComment([FromRoute] int id)
    {
        var comment = _commentRepository.GetById(id);
        if (!_permissionCheck.CanViewQuestion(comment.TypeId))
            throw new Exception("No permission to view this comment");

        if (comment.AnswerTo != null)
            return GetComment(comment.AnswerTo, showSettled: true);

        return GetComment(comment, showSettled: true);
    }

    private CommentJson GetComment(Comment c, bool showSettled = false)
    {
        var comment = new CommentJson
        {
            Id = c.Id,
            Title = c.Title,
            Text = c.Text,

            CreatorName = c.Creator.Name,
            CreatorId = c.Creator.Id,
            CreatorImgUrl = new UserImageSettings(c.Creator.Id, _httpContextAccessor)
                .GetUrl_128px_square(c.Creator)
                .Url,

            CreationDate = c.DateCreated.ToString("U"),
            CreationDateNiceText = DateTimeUtils.TimeElapsedAsText(c.DateCreated),

            ShouldBeImproved = c.ShouldImprove,
            ShouldBeDeleted = c.ShouldRemove,
            ShouldReasons = TrueOrFalse.ShouldReasons.ByKeys(c.ShouldKeys),

            IsSettled = c.IsSettled,
            ShowSettledAnswers = showSettled
        };

        if (c.Answers != null)
        {
            if (showSettled)
            {
                comment.answers = c.Answers
                    .OrderBy(x => x.DateCreated)
                    .Select(x => GetComment(x, true)).ToArray();
            }
            else
            {
                comment.answers = c.Answers
                    .Where(x => !x.IsSettled)
                    .OrderBy(x => x.DateCreated)
                    .Select(x => GetComment(x)).ToArray();
            }
            comment.answersSettledCount = c.Answers.Count(x => x.IsSettled);
        }
        return comment;
    }

    public record struct Comments(CommentJson[] SettledComments, CommentJson[] UnsettledComments);

    public record struct CommentJson(
        int Id,
        string Title,
        string Text,
        string CreatorName,
        int CreatorId,
        string CreatorImgUrl,
        string CreationDate,
        string CreationDateNiceText,
        bool ShouldBeImproved,
        bool ShouldBeDeleted,
        bool IsSettled,
        List<string> ShouldReasons,
        CommentJson[] answers,
        int answersSettledCount,
        bool ShowSettledAnswers
    );
}

