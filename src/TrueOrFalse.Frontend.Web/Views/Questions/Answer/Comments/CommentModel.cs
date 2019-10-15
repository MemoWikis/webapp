using System.Collections.Generic;
using System.Linq;

public class CommentModel : BaseModel
{
    /// <summary>Comment.Id</summary>
    public int Id;
    public string CreatorName;
    public string CreationDate;
    public string CreationDateNiceText;
    public string ImageUrl;
    public string Text;

    public UserTinyModel Creator;

    public bool ShouldBeImproved;
    public bool ShouldBeDeleted;

    public bool IsSettled;

    public List<string> ShouldReasons; 

    public IEnumerable<CommentModel> Answers = new List<CommentModel>();
    public int AnswersSettledCount = 0;
    public bool ShowSettledAnswers;

    public CommentModel(Comment comment, bool showSettled = false)
    {
        
        Id = comment.Id;
        Creator = new UserTinyModel(comment.Creator);
        CreatorName = Creator.Name;
        CreationDate = comment.DateCreated.ToString("U");
        CreationDateNiceText = DateTimeUtils.TimeElapsedAsText(comment.DateCreated);
        ImageUrl = new UserImageSettings(Creator.Id).GetUrl_128px_square(Creator).Url;
        Text = comment.Text;

        ShouldBeImproved = comment.ShouldImprove;
        ShouldBeDeleted = comment.ShouldRemove;
        ShouldReasons = TrueOrFalse.ShouldReasons.ByKeys(comment.ShouldKeys);
        IsSettled = comment.IsSettled;
        ShowSettledAnswers = showSettled;

        if (comment.Answers != null)
        {
            if (ShowSettledAnswers)
            {
                Answers = comment.Answers
                    .OrderBy(x => x.DateCreated)
                    .Select(x => new CommentModel(x, showSettled));
            }
            else
            {
                Answers = comment.Answers
                    .Where(x => !x.IsSettled)
                    .OrderBy(x => x.DateCreated)
                    .Select(x => new CommentModel(x));
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
}