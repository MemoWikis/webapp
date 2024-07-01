using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NHibernate;
using Seedworks.Lib.Persistence;
using ISession = NHibernate.ISession;

public class CommentRepository : RepositoryDb<Comment>
{
    private readonly MessageRepo _messageRepo;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IActionContextAccessor _actionContextAccessor;

    public CommentRepository(ISession session,
        MessageRepo messageRepo,
        QuestionReadingRepo questionReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        IActionContextAccessor actionContextAccessor) : base(session)
    {
        _messageRepo = messageRepo;
        _questionReadingRepo = questionReadingRepo;
        _httpContextAccessor = httpContextAccessor;
        _actionContextAccessor = actionContextAccessor;
    }

    public override void Create(Comment comment)
    {
        base.Create(comment);
        CommentMsg.Send(comment,
            _questionReadingRepo,
            _messageRepo,
            _httpContextAccessor,
            _actionContextAccessor);
    }

    public IList<Comment> GetForDisplay(int questionId)
    {
        return _session.QueryOver<Comment>()
            .Where(x => x.TypeId == questionId &&
                        x.Type == CommentType.AnswerQuestion &&
                        x.AnswerTo == null)
            .Fetch(x => x.Answers).Eager
            .JoinQueryOver(x => x.Creator) 
            .Fetch(u => u.Creator).Eager 
            .List()
            .GroupBy(x => x.Id)
            .Select(x => x.First())
            .ToList();
    }

    public int GetCommentsCount(int questionId)
    {
       var count =  _session.QueryOver<Comment>()
            .Where(x => x.TypeId == questionId &&
                        x.Type == CommentType.AnswerQuestion &&
                        x.AnswerTo == null && x.IsSettled == false)
            .List<Comment>()
            .Count(); 
       return count;
    }


    public void DeleteForQuestion(int questionId)
    {
        Session.CreateSQLQuery("DELETE FROM comment WHERE TYPE = :commentType AND TypeId = :questionId")
                .SetParameter("commentType", CommentType.AnswerQuestion)
                .SetParameter("questionId", questionId)
                .ExecuteUpdate();
    }

    
    public void UpdateIsSettled(int commentId, bool settled)
    {
        var comment = GetById(commentId);
        comment.IsSettled = settled;
        Update(comment);
    }
}