using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class CommentsStoreController : Controller
{
    private readonly CommentRepository _commentRepository;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CommentsStoreController(SessionUser sessionUser,
        CommentRepository commentRepository,
        UserReadingRepo userReadingRepo,
        IHttpContextAccessor httpContextAccessor) 
    {
        _commentRepository = commentRepository;
        _userReadingRepo = userReadingRepo;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public JsonResult GetAllComments([FromRoute] int id)
    {
        var _comments = _commentRepository.GetForDisplay(id);
        var commentHelper = new AddCommentService(_commentRepository, _userReadingRepo, _httpContextAccessor);
        var settledComments = _comments.Where(c => c.IsSettled).Select(c => commentHelper.GetComment(c)).ToArray();
        var unsettledComments = _comments.Where(c => !c.IsSettled).Select(c => commentHelper.GetComment(c)).ToArray();

        return Json(new
        {
            settledComments = settledComments,
            unsettledComments = unsettledComments
        });
    }
}

