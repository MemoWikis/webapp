using System;
using System.Web.Mvc;

public class AnswerCommentsController : BaseController
{

    [HttpPost]
    public ActionResult SaveComment(
        int questionId, 
        string text,
        bool? typeImprovement,
        bool? typeRemove,
        string typeKeys)
    {
        var comment = new Comment();
        comment.Type = CommentType.AnswerQuestion;
        comment.TypeId = questionId;
        comment.Text = text;
        comment.Creator = _sessionUser.User;

        if(typeImprovement.HasValue)
            comment.ShouldImprove = typeImprovement.Value;

        if (typeRemove.HasValue)
            comment.ShouldRemove = typeRemove.Value;

        if (!String.IsNullOrEmpty(typeKeys))
            comment.ShouldKeys = typeKeys;

        Resolve<CommentRepository>().Create(comment);

        return View("~/Views/Questions/Answer/Comments/Comment.ascx",
            new CommentModel(comment));
    }

    [HttpPost]
    public ActionResult SaveAnswer(int commentId, string text)
    {
        var commentRepo = Resolve<CommentRepository>();
        var parentComment = commentRepo.GetById(commentId);

        var comment = new Comment();
        comment.Type = CommentType.AnswerQuestion;
        comment.TypeId = parentComment.TypeId;
        comment.AnswerTo = parentComment;
        comment.Text = text;
        comment.Creator = _sessionUser.User;

        commentRepo.Create(comment);

        return View("~/Views/Questions/Answer/Comments/CommentAnswer.ascx",
            new CommentModel(comment));
    }

    [HttpPost]
    [AccessOnlyAsAdmin]
    public void MarkCommentAsSettled(int commentId)
    {
        Sl.R<CommentRepository>().UpdateIsSettled(commentId, true);
        //todo: inform comment-creator with message of changed status
    }

    [HttpPost]
    [AccessOnlyAsAdmin]
    public void MarkCommentAsUnsettled(int commentId)
    {
        Sl.R<CommentRepository>().UpdateIsSettled(commentId, false);
        //todo: inform comment-creator with message of changed status
    }

    public ActionResult GetAnswerHtml()
    {
        return View("~/Views/Questions/Answer/Comments/CommentAnswerAdd.ascx", 
            new CommentAnswerAddModel());
    }
}