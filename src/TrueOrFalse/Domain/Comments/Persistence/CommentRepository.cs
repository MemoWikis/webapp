using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using Seedworks.Lib.Persistence;


public class CommentRepository : RepositoryDb<Comment>
{
    public CommentRepository(ISession session) : base(session)
    {
    }
}

