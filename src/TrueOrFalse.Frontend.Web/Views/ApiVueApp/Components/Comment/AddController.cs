using System.Web.Mvc;

namespace VueApp;

public class CommentAddController : BaseController
{
    public CommentAddController(SessionUser sessionUser) :base(sessionUser)
    {
        
    }
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public bool SaveComment(int id, string text, string title)
    {
        var comment = new Comment();
        comment.Type = CommentType.AnswerQuestion;
        comment.TypeId = id;
        comment.Text = text;
        comment.Title = title;
        comment.Creator = Sl.UserRepo.GetById(UserId);

        CommentRepository.Create(comment);
        return true;
    }
}