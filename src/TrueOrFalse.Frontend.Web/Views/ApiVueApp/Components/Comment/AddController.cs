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


    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public CommentModel? SaveAnswer([FromBody]AddAnswerType addAnswerType)
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

        return new CommentModel(comment, _httpContextAccessor);
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

}
public readonly record struct AddCommentJson(int id, string text, string title);
public readonly record struct AddAnswerType(int commentId, string text);
