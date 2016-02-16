using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Seedworks.Lib.Persistence;

public class CommentRepository : RepositoryDb<Comment>
{
    public CommentRepository(ISession session) : base(session)
    {
    }

    public override void Create(Comment comment)
    {
        base.Create(comment);
        CommentMsgSend.Run(comment);
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
}