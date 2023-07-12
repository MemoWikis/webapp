using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Seedworks.Lib.Persistence;

public class CommentRepository : RepositoryDb<Comment>
{
    private readonly QuestionRepo _questionRepo;
    private readonly MessageRepo _messageRepo;

    public CommentRepository(ISession session,
        QuestionRepo questionRepo, 
        MessageRepo messageRepo) : base(session)
    {
        _questionRepo = questionRepo;
        _messageRepo = messageRepo;
    }

    public override void Create(Comment comment)
    {
        base.Create(comment);
        CommentMsg.Send(comment, _questionRepo, _messageRepo);
    }

    public IList<Comment> GetForDisplay(int questionId)
    {
        return _session.QueryOver<Comment>()
            .Where(
                x => x.TypeId == questionId && 
                x.Type == CommentType.AnswerQuestion &&
                x.AnswerTo == null)
            .Fetch(x => x.Answers).Eager
            .List<Comment>()
            .GroupBy(x => x.Id)
            .Select(x => x.First())
            .ToList();
    }

    public void DeleteForQuestion(int questionId)
    {
        Session.CreateSQLQuery("DELETE FROM comment WHERE TYPE = :commentType AND TypeId = :questionId")
                .SetParameter("commentType", CommentType.AnswerQuestion)
                .SetParameter("questionId", questionId)
                .ExecuteUpdate();
    }
}