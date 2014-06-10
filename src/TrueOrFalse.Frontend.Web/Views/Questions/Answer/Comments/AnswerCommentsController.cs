﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class AnswerCommentsController : BaseController
{

    [HttpPost]
    public ActionResult SaveComment(
        int questionId, 
        string text,
        bool? typeImprovement,
        bool? typeRemove,
        int? typeId)
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

    public ActionResult GetAnswerHtml()
    {
        return View("~/Views/Questions/Answer/Comments/CommentAnswerAdd.ascx", 
            new CommentAnswerAddModel());
    }
}