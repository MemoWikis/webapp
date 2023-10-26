using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class CommentModel
{
    /// <summary>Comment.Id</summary>
    public int Id;
    public string CreatorName;
    public string CreationDate;
    public string CreationDateNiceText;
    public string CreatorUrl;
    public string ImageUrl;
    public string Title;
    public string Text;


    public bool ShouldBeImproved;
    public bool ShouldBeDeleted;

    public bool IsSettled;

    public List<string> ShouldReasons;

    public IEnumerable<CommentModel> Answers = new List<CommentModel>();
    public int AnswersSettledCount = 0;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public bool ShowSettledAnswers;

    public CommentModel(Comment comment,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        bool showSettled = false)
    {

        Id = comment.Id;
        CreatorName = comment.Creator.Name;
        CreationDate = comment.DateCreated.ToString("U");
        CreationDateNiceText = DateTimeUtils.TimeElapsedAsText(comment.DateCreated);
        CreatorUrl = "/Nutzer/" + comment.Creator.Name + "/" + comment.Creator.Id;
        ImageUrl = new UserImageSettings(comment.Creator.Id, _httpContextAccessor).GetUrl_128px_square(comment.Creator).Url;
        Title = comment.Title ?? "";
        Text = comment.Text;

        ShouldBeImproved = comment.ShouldImprove;
        ShouldBeDeleted = comment.ShouldRemove;
        ShouldReasons = TrueOrFalse.ShouldReasons.ByKeys(comment.ShouldKeys);
        IsSettled = comment.IsSettled;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        ShowSettledAnswers = showSettled;

        if (comment.Answers != null)
        {
            if (ShowSettledAnswers)
            {
                Answers = comment.Answers
                    .OrderBy(x => x.DateCreated)
                    .Select(x => new CommentModel(x, _httpContextAccessor, _webHostEnvironment, showSettled));
            }
            else
            {
                Answers = comment.Answers
                    .Where(x => !x.IsSettled)
                    .OrderBy(x => x.DateCreated)
                    .Select(x => new CommentModel(x, _httpContextAccessor, _webHostEnvironment));
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


