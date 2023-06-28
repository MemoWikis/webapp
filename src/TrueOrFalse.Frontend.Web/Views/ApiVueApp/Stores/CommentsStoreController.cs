using System.Web.Mvc;
using System.Linq;

namespace VueApp;

public class CommentsStoreController : BaseController
{
    public CommentsStoreController(SessionUser sessionUser) :base(sessionUser)
    {
        
    }

    [HttpGet]
    public JsonResult GetAllComments(int questionId)
    {
        var _comments = Resolve<CommentRepository>().GetForDisplay(questionId);

        var settledComments = _comments.Where(c => c.IsSettled).Select(c => CommentHelper.GetComment(c)).ToArray();
        var unsettledComments = _comments.Where(c => !c.IsSettled).Select(c => CommentHelper.GetComment(c)).ToArray();

        return Json(new
        {
            settledComments = settledComments,
            unsettledComments = unsettledComments
        }, JsonRequestBehavior.AllowGet);
    }
}

