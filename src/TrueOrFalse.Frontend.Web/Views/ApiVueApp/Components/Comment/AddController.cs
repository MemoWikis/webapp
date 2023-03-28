using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Web;

namespace VueApp;

public class CommentAddController : BaseController
{
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public bool SaveComment(int id, string text, string title)
    {
        var comment = new Comment();
        comment.Type = CommentType.AnswerQuestion;
        comment.TypeId = id;
        comment.Text = text;
        comment.Title = title;
        comment.Creator = Sl.UserRepo.GetById(SessionUser.UserId);

        Resolve<CommentRepository>().Create(comment);
        return true;
    }
}