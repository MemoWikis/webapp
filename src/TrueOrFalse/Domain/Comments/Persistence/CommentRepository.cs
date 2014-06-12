using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using NHibernate;
using Seedworks.Lib.Persistence;
using TrueOrFalse;


public class CommentRepository : RepositoryDb<Comment>
{
    public CommentRepository(ISession session) : base(session)
    {
    }

    public override void Create(Comment comment)
    {
        base.Create(comment);
        Sl.Resolve<SendCommentMsg>().Run(comment);
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