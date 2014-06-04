using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


public class AnswerCommentsController : BaseController
{

    [HttpPost]
    public ActionResult AddComment(int questionId, string text)
    {
        var comment = new Comment();
        comment.Type = CommentType.AnswerQuestion;
        comment.TypeId = questionId;
        comment.Text = text;
        comment.Creator = _sessionUser.User;

        Resolve<CommentRepository>().Create(comment);

        return View("~/Views/Questions/Answer/Comments/Comment.ascx",
            new CommentModel(comment));
    }

}
