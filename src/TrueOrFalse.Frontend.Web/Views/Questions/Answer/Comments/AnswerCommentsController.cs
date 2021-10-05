using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using WebGrease.Css.Extensions;


public class AnswerCommentsController : BaseController
{

    [HttpPost]
    public CommentModel SaveComment(
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

        if (typeImprovement.HasValue)
            comment.ShouldImprove = typeImprovement.Value;

        if (typeRemove.HasValue)
            comment.ShouldRemove = typeRemove.Value;

        if (!String.IsNullOrEmpty(typeKeys))
            comment.ShouldKeys = typeKeys;

        Resolve<CommentRepository>().Create(comment);

        var commentModel = new CommentModel(comment);

        return commentModel;
    }

    [HttpPost]
    public bool SaveAnswer(int commentId, string text)
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

        return true;
    }

    public ActionResult GetAnswerHtml()
    {
        return View("~/Views/Questions/Answer/Comments/CommentAnswerAdd.ascx");
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

    //[HttpPost]
    //public ActionResult GetAllAnswersInclSettledHtml(int commentId)
    //{
    //    var comment = Resolve<CommentRepository>().GetById(commentId);

    //    return View("~/Views/Questions/Answer/Comments/Comment.vue.ascx",
    //        new CommentModel(comment, true));
    //}

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


    [HttpPost]
    public String GetCurrentUserImgUrl()
    {
        var currentUserImageUrl = new UserImageSettings(_sessionUser.User.Id).GetUrl_128px_square(_sessionUser.User).Url;
        return currentUserImageUrl;
    }

    [HttpPost]
    public int GetCurrentUserId()
    {
        return _sessionUser.User.Id;
    }

    [HttpPost]
    public String GetUserImgUrl(int userId)
    {
        var userImageUrl = new UserImageSettings(userId).GetUrl_128px_square(Sl.UserRepo.GetById(userId)).Url;
        return userImageUrl;
    }

    [HttpPost]
    public String GetComments(int questionId)
    {
        var _comments = Resolve<CommentRepository>().GetForDisplay(questionId);
        var commentsList = new List<CommentModel>();
        foreach (var comment in _comments)
        {
            if (!comment.IsSettled)
            {
                commentsList.Add(new CommentModel(comment));
            }
        }
        var json = JsonConvert.SerializeObject(commentsList.ToArray(), Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        return json;
    }

    [HttpPost]
    public String GetSettledComments(int questionId)
    {
        var _comments = Resolve<CommentRepository>().GetForDisplay(questionId);
        var commentsList = new List<CommentModel>();
        foreach (var comment in _comments)
        {
            if (comment.IsSettled)
            {
                commentsList.Add(new CommentModel(comment));
            }
        }
        var json = JsonConvert.SerializeObject(commentsList.ToArray(), Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        return json;
    }
}