using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Seedworks.Lib.Persistence;

public class CommentRepository : RepositoryDb<Comment>
{
    private readonly MessageRepo _messageRepo;
    private readonly QuestionReadingRepo _questionReadingRepo;

    public CommentRepository(ISession session,
        MessageRepo messageRepo,
        QuestionReadingRepo questionReadingRepo) : base(session)
    {
        _messageRepo = messageRepo;
        _questionReadingRepo = questionReadingRepo;
    }

    public override void Create(Comment comment)
    {
        base.Create(comment);
        CommentMsg.Send(comment, _questionReadingRepo,_messageRepo);
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