public class CommentModel
{
    /// <summary>Comment.Id</summary>
    public int Id;

    public string CreatorName;
    public string CreationDate;
    public string CreationDateNiceText;
    public string CreatorUrl;
    public string CreatorImgUrl;
    public string Title;
    public string Text;

    public bool ShouldBeImproved;
    public bool ShouldBeDeleted;

    public bool IsSettled;

    public List<string> ShouldReasons;

    public IEnumerable<CommentModel> Answers = new List<CommentModel>();
    public int AnswersSettledCount = 0;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public bool ShowSettledAnswers;

    public CommentModel(Comment comment,
        IHttpContextAccessor httpContextAccessor,
        bool showSettled = false)
    {
        Id = comment.Id;
        CreatorName = comment.Creator.Name;
        CreationDate = comment.DateCreated.ToString("U");
        CreationDateNiceText = DateTimeUtils.TimeElapsedAsText(comment.DateCreated);
        CreatorUrl = "/Nutzer/" + comment.Creator.Name + "/" + comment.Creator.Id;
        Title = comment.Title ?? "";
        Text = comment.Text;
        ShouldBeImproved = comment.ShouldImprove;
        ShouldBeDeleted = comment.ShouldRemove;
        ShouldReasons = global::ShouldReasons.ByKeys(comment.ShouldKeys);
        IsSettled = comment.IsSettled;
        _httpContextAccessor = httpContextAccessor;
        ShowSettledAnswers = showSettled;
        CreatorImgUrl = new UserImageSettings(comment.Creator.Id, _httpContextAccessor)
            .GetUrl_128px_square(comment.Creator)
            .Url;


        if (comment.Answers != null)
        {
            if (ShowSettledAnswers)
            {
                Answers = comment.Answers
                    .OrderBy(x => x.DateCreated)
                    .Select(x => new CommentModel(x, _httpContextAccessor, showSettled));
            }
            else
            {
                Answers = comment.Answers
                    .Where(x => !x.IsSettled)
                    .OrderBy(x => x.DateCreated)
                    .Select(x => new CommentModel(x, _httpContextAccessor));
            }
            AnswersSettledCount = comment.Answers.Count(x => x.IsSettled);
        }
    }
}

public static class CommentModelListExt
{
    public static int GetTotalCount(this IList<CommentModel> comments)
    {
        var totalCount = 0;

        foreach (var comment in comments)
        {
            totalCount++;
            totalCount += comment.Answers.Count();
        }

        return totalCount;
    }
    public static int GetUnsettledCount(this IList<CommentModel> comments)
    {
        var totalCount = 0;

        foreach (var comment in comments)
        {
            if (!comment.IsSettled)
            {
                totalCount++;
            }
        }

        return totalCount;
    }
}


