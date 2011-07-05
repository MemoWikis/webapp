using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Seedworks.Lib.Persistance;

namespace TrueOrFalse.Core
{
    public class KeywordRepository : RepositoryDb<Keyword>
    {
        public KeywordRepository(ISession session) : base(session)
        {
        }
    }
}
