using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class CommentsStoreController : BaseController
{
    private readonly CommentRepository _commentRepository;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CommentsStoreController(SessionUser sessionUser,
        CommentRepository commentRepository,
        UserReadingRepo userReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment) : base(sessionUser)
    {
        _commentRepository = commentRepository;
        _userReadingRepo = userReadingRepo;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public JsonResult GetAllComments([FromRoute] int questionId)
    {
        var _comments = _commentRepository.GetForDisplay(questionId);
        var commentHelper = new CommentHelper(_commentRepository, _userReadingRepo, _httpContextAccessor, _webHostEnvironment);
        var settledComments = _comments.Where(c => c.IsSettled).Select(c => commentHelper.GetComment(c)).ToArray();
        var unsettledComments = _comments.Where(c => !c.IsSettled).Select(c => commentHelper.GetComment(c)).ToArray();

        return Json(new
        {
            settledComments = settledComments,
            unsettledComments = unsettledComments
        });
    }
}

