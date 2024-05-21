using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class CommentAddController(
    SessionUser _sessionUser,
    CommentRepository _commentRepository, 
    UserReadingRepo _userReadingRepo) : Controller
{
    

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public bool SaveComment([FromBody] AddCommentJson json)
    {
      SaveComment(CommentType.AnswerQuestion, json, _sessionUser.UserId);
        return true;
    }

    private void SaveComment(CommentType type, AddCommentJson json, int userId )
    {
        var comment = new Comment();
        comment.Type = type;
        comment.TypeId = json.id;
        comment.Text = json.text;
        comment.Title = json.title;
        comment.Creator = _userReadingRepo.GetById(userId);

        _commentRepository.Create(comment);
    }

}
public readonly record struct AddCommentJson(int id, string text, string title);
