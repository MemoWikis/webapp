using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Web;

namespace VueApp;

public class CommentController : BaseController
{

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult SaveAnswer(int commentId, string text)
    {
        var commentRepo = Resolve<CommentRepository>();
        var parentComment = commentRepo.GetById(commentId);

        if (parentComment.IsSettled)
        {
            return null;
        }

        var comment = new Comment();
        comment.Type = CommentType.AnswerQuestion;
        comment.TypeId = parentComment.TypeId;
        comment.AnswerTo = parentComment;
        comment.Text = text;
        comment.Creator = Sl.UserRepo.GetById(SessionUserLegacy.UserId);

        commentRepo.Create(comment);

        return Json(CommentHelper.GetComment(comment));
    }

    [HttpPost]
    public bool MarkCommentAsSettled(int commentId)
    {
        Sl.R<CommentRepository>().UpdateIsSettled(commentId, true);
        CommentMarkedAsSettledMsg.Send(commentId);
        var comment = Sl.R<CommentRepository>().GetById(commentId);
        if (comment.IsSettled)
            return true;
        return false;
    }

    [HttpPost]
    public bool MarkCommentAsUnsettled(int commentId)
    {
        Sl.R<CommentRepository>().UpdateIsSettled(commentId, false);
        //todo: inform comment-creator and question-owner with message of changed status
        var comment = Sl.R<CommentRepository>().GetById(commentId);
        if (comment.IsSettled)
            return true;
        return false;
    }
}