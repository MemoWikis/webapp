using System.Collections.Generic;
using System.Linq;
using TrueOrFalse;

public class CommentModel : BaseModel
{
    /// <summary>Comment.Id</summary>
    public int Id;
    public string CreatorName;
    public string CreationDate;
    public string CreationDateNiceText;
    public string ImageUrl;
    public string Text;

    public User Creator;

    public bool ShouldBeImproved;
    public bool ShouldBeDeleted;

    public bool IsSettled;

    public List<string> ShouldReasons; 

    public IEnumerable<CommentModel> Answers = new List<CommentModel>(); 

    public CommentModel(Comment comment)
    {
        Id = comment.Id;
        Creator = comment.Creator;
        CreatorName = comment.Creator.Name;
        CreationDate = comment.DateCreated.ToString("U");
        CreationDateNiceText = DateTimeUtils.TimeElapsedAsText(comment.DateCreated);
        ImageUrl = new UserImageSettings(comment.Creator.Id).GetUrl_128px_square(comment.Creator).Url;
        Text = comment.Text;

        ShouldBeImproved = comment.ShouldImprove;
        ShouldBeDeleted = comment.ShouldRemove;
        ShouldReasons = TrueOrFalse.ShouldReasons.ByKeys(comment.ShouldKeys);
        IsSettled = comment.IsSettled;

        if(comment.Answers != null)
            Answers = comment.Answers
                        .OrderBy(x => x.DateCreated)
                        .Select(x => new CommentModel(x));
    }
}
