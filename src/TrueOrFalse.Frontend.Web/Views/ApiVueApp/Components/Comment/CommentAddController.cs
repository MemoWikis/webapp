using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class CommentAddController(
    SessionUser _sessionUser,
    CommentRepository _commentRepository,
    UserReadingRepo _userReadingRepo,
    IHttpContextAccessor _httpContextAccessor) : Controller
{
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public bool SaveComment([FromBody] AddCommentJson json)
    {
        SaveComment(CommentType.AnswerQuestion, json, _sessionUser.UserId);
        return true;
    }

    private void SaveComment(CommentType type, AddCommentJson json, int userId)
    {
        var comment = new Comment();
        comment.Type = type;
        comment.TypeId = json.id;
        comment.Text = json.text;
        comment.Title = json.title;
        comment.Creator = _userReadingRepo.GetById(userId);

        _commentRepository.Create(comment);
    }

    public record struct SaveAnswerResult(int Id, 
        string CreatorName, 
        string CreationDate,
        string CreationDateNiceText,
        string CreatorImgUrl,
        string Title,
        string Text,
        bool ShouldBeImproved,
        bool ShouldBeDeleted,
        List<string> ShouldReasons,
        bool IsSettled,
        List<CommentModel> Answers,
        int AnswersSettledCount,
        bool ShowSettledAnswers,
        string CreatorUrl); 

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public SaveAnswerResult? SaveAnswer([FromBody]AddAnswerType addAnswerType)
    {
        var parentComment = _commentRepository.GetById(addAnswerType.commentId);

        if (parentComment.IsSettled)
        {
            return null;
        }

        var comment = new Comment();
        comment.Type = CommentType.AnswerQuestion;
        comment.TypeId = parentComment.TypeId;
        comment.AnswerTo = parentComment;
        comment.Text = addAnswerType.text;
        comment.Creator = _userReadingRepo.GetById(_sessionUser.UserId);

        _commentRepository.Create(comment);
        var commentModel = new CommentModel(comment, _httpContextAccessor);
        return SetAnswerResult(commentModel);

    }

    [HttpPost]
    public void MarkCommentAsSettled(int commentId)
    {
        _commentRepository.UpdateIsSettled(commentId, true);
        //CommentMarkedAsSettledMsg.Send(commentId);
    }

    [HttpPost]
    public void MarkCommentAsUnsettled(int commentId)
    {
        _commentRepository.UpdateIsSettled(commentId, false);
    }


    private SaveAnswerResult? SetAnswerResult(CommentModel commentModel)
    {
        return new SaveAnswerResult
        {
            Answers = commentModel.Answers.ToList(),
            AnswersSettledCount = commentModel.AnswersSettledCount,
            CreationDate = commentModel.CreationDate,
            CreationDateNiceText = commentModel.CreationDateNiceText,
            CreatorName = commentModel.CreatorName,
            CreatorUrl = commentModel.CreatorUrl,
            Id = commentModel.Id,
            IsSettled = commentModel.IsSettled,
            ShouldBeDeleted = commentModel.ShouldBeDeleted,
            ShouldBeImproved = commentModel.ShouldBeImproved,
            ShouldReasons = commentModel.ShouldReasons,
            ShowSettledAnswers = commentModel.ShowSettledAnswers,
            Text = commentModel.Text,
            Title = commentModel.Title,
            CreatorImgUrl = commentModel.CreatorImgUrl

        };
    }
}  
public readonly record struct AddCommentJson(int id, string text, string title);
public readonly record struct AddAnswerType(int commentId, string text);
