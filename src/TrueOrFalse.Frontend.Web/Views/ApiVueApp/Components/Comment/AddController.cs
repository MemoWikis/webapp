using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class CommentAddController(SessionUser _sessionUser, AddCommentService _addCommentService) : Controller
{
    

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public bool SaveComment([FromBody] AddCommentJson json)
    {
        _addCommentService.SaveComment(CommentType.AnswerQuestion, json, _sessionUser.UserId);
        return true;
    }
}
public readonly record struct AddCommentJson(int id, string text, string title);
