using System.Web.Mvc;
using System.Linq;

namespace VueApp;

public class CommentsStoreController : BaseController
{
    private readonly CommentRepository _commentRepository;

    public CommentsStoreController(SessionUser sessionUser,
        CommentRepository commentRepository) : base(sessionUser)
    {
        _commentRepository = commentRepository;
    }

    [HttpGet]
    public JsonResult GetAllComments(int questionId)
    {
        var _comments = _commentRepository.GetForDisplay(questionId);

        var settledComments = _comments.Where(c => c.IsSettled).Select(c => CommentHelper.GetComment(c)).ToArray();
        var unsettledComments = _comments.Where(c => !c.IsSettled).Select(c => CommentHelper.GetComment(c)).ToArray();

        return Json(new
        {
            settledComments = settledComments,
            unsettledComments = unsettledComments
        }, JsonRequestBehavior.AllowGet);
    }
}

