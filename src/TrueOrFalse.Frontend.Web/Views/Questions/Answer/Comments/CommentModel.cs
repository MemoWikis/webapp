using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Mapping;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

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

    public List<string> ShouldReasons; 

    public IEnumerable<CommentModel> Answers = new List<CommentModel>(); 

    public CommentModel(Comment comment)
    {
        Id = comment.Id;
        Creator = comment.Creator;
        CreatorName = comment.Creator.Name;
        CreationDate = comment.DateCreated.ToString("U");
        CreationDateNiceText = TimeElapsedAsText.Run(comment.DateCreated);
        ImageUrl = new UserImageSettings(comment.Creator.Id).GetUrl_128px_square(comment.Creator.EmailAddress).Url;
        Text = comment.Text;

        ShouldBeImproved = comment.ShouldImprove;
        ShouldBeDeleted = comment.ShouldRemove;
        ShouldReasons = TrueOrFalse.ShouldReasons.ByKeys(comment.ShouldKeys);

        if(comment.Answers != null)
            Answers = comment.Answers
                        .OrderBy(x => x.DateCreated)
                        .Select(x => new CommentModel(x));
    }
}
