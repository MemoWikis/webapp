﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

    public CommentModel(Comment comment)
    {
        Id = comment.Id;
        CreatorName = comment.Creator.Name;
        CreationDate = comment.DateCreated.ToString("U");
        CreationDateNiceText = TimeElapsedAsText.Run(comment.DateCreated);
        ImageUrl = new UserImageSettings(comment.Creator.Id).GetUrl_128px_square(comment.Creator.EmailAddress).Url;
        Text = comment.Text;
    }
}
