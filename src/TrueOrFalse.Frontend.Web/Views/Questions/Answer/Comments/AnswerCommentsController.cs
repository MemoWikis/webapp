using System;
using System.Collections.Generic;
using System.Text;
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

        return View("~/Views/Questions/Answer/Comments/Comment.vue.ascx",
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

        return View("~/Views/Questions/Answer/Comments/CommentAnswer.vue.ascx",
            new CommentModel(comment));
    }

    public ActionResult GetAnswerHtml()
    {
        return View("~/Views/Questions/Answer/Comments/CommentAnswerAdd.vue.ascx",
            new CommentAnswerAddModel());
    }

    [HttpPost]
    [AccessOnlyAsAdmin]
    public void MarkCommentAsSettled(int commentId)
    {
        Sl.R<CommentRepository>().UpdateIsSettled(commentId, true);
        CommentMarkedAsSettledMsg.Send(commentId);
    }

    [HttpPost]
    [AccessOnlyAsAdmin]
    public void MarkCommentAsUnsettled(int commentId)
    {
        Sl.R<CommentRepository>().UpdateIsSettled(commentId, false);
        //todo: inform comment-creator and question-owner with message of changed status
    }

    [HttpPost]
    public ActionResult GetAllAnswersInclSettledHtml(int commentId)
    {
        var comment = Resolve<CommentRepository>().GetById(commentId);

        return View("~/Views/Questions/Answer/Comments/Comment.vue.ascx",
            new CommentModel(comment, true));
    }

    [HttpPost]
    public List<CommentModel> GetAllCommentsInclSettledHtml(int questionId)
    {
        var comments = Resolve<CommentRepository>().GetForDisplay(questionId);

        var result = new List<CommentModel>();
        foreach (var comment in comments)
        {
            result.Add(new CommentModel(comment));
        }
        return result;
    }

}