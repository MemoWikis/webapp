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
    public CommentModel SaveComment(SaveCommentJson saveCommentJson)
    {
        var comment = new Comment();
        comment.Type = CommentType.AnswerQuestion;
        comment.TypeId = saveCommentJson.questionId;
        comment.Text = saveCommentJson.text;
        comment.Title = saveCommentJson.title;
        comment.Creator = _sessionUser.User;


        Resolve<CommentRepository>().Create(comment);

        var commentModel = new CommentModel(comment);

        return commentModel;
    }

    public class SaveCommentJson
    {
        
        public int questionId { get; set; }
        public string text { get; set; }
    public string title { get; set; }
    }

    [HttpPost]
    public bool SaveAnswer(int commentId, string text)
    {
        var commentRepo = Resolve<CommentRepository>();
        var parentComment = commentRepo.GetById(commentId);

        if (parentComment.IsSettled)
        {
            return false;
        }

        var comment = new Comment();
        comment.Type = CommentType.AnswerQuestion;
        comment.TypeId = parentComment.TypeId;
        comment.AnswerTo = parentComment;
        comment.Text = text;
        comment.Creator = _sessionUser.User;

        commentRepo.Create(comment);

        return true;
    }

    [HttpPost]
    public void MarkCommentAsSettled(int commentId)
    {
        Sl.R<CommentRepository>().UpdateIsSettled(commentId, true);
        CommentMarkedAsSettledMsg.Send(commentId);
    }

    [HttpPost]
    public void MarkCommentAsUnsettled(int commentId)
    {
        Sl.R<CommentRepository>().UpdateIsSettled(commentId, false);
        //todo: inform comment-creator and question-owner with message of changed status
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


    [HttpPost]
    public String GetCurrentUserImgUrl()
    {
        if (_sessionUser.User != null)
        {
            var currentUserImageUrl =
                new UserImageSettings(_sessionUser.User.Id).GetUrl_128px_square(_sessionUser.User).Url;
            return currentUserImageUrl;
        }
        else
        {
            return null;
        }
    }

    [HttpPost]
    public int GetCurrentUserId()
    {
        if (_sessionUser.User != null)
        {
            return _sessionUser.User.Id;
        }
        else
        {
            return -1;
        }
    }

    [HttpPost]
    public string GetCurrentUserName()
    {
        if (_sessionUser.User != null)
        {
                return _sessionUser.User.Name;
        }
        else
        {
            return null;
        }
    }

    [HttpPost]
    public bool GetCurrentUserAdmin()
    {
        if (_sessionUser.User != null)
        {
                    return _sessionUser.User.IsInstallationAdmin;
        }
        else
        {
            return false;
        }
    }

    [HttpPost]
    public String GetUserImgUrl(int userId)
    {
        if (_sessionUser.User != null)
        {
                        var userImageUrl = new UserImageSettings(userId).GetUrl_128px_square(Sl.UserRepo.GetById(userId)).Url;
        return userImageUrl;
        }
        else
        {
            return null;
        }
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