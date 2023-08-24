using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class CommentAddController : BaseController
{
    private readonly CommentHelper _commentHelper;

    public CommentAddController(SessionUser sessionUser, CommentHelper commentHelper) :base(sessionUser)
    {
        _commentHelper = commentHelper;
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public bool SaveComment(int id, string text, string title)
    {
        _commentHelper.SaveComment(CommentType.AnswerQuestion, id, text,title, UserId);
        return true;
    }
}