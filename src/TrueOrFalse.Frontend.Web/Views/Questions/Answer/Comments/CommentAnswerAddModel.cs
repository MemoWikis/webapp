using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;


public class CommentAnswerAddModel : BaseModel
{
    public string AuthorImageUrl;

    public CommentAnswerAddModel()
    {
        var user = _sessionUser.User;
        AuthorImageUrl = new UserImageSettings(user.Id).GetUrl_128px_square(user.EmailAddress).Url;
    }
}
