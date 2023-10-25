using HelperClassesControllers;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class CommentAddController : BaseController
{
    private readonly AddCommentService _addCommentService;

    public CommentAddController(SessionUser sessionUser, AddCommentService addCommentService) :base(sessionUser)
    {
        _addCommentService = addCommentService;
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public bool SaveComment([FromBody] AddCommentHelper.AddJson json)
    {
        _addCommentService.SaveComment(CommentType.AnswerQuestion, json, UserId);
        return true;
    }
}