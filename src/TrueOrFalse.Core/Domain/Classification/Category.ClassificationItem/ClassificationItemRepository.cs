using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Seedworks.Lib.Persistance;

namespace TrueOrFalse.Core
{
    public class ClassificationItemRepository : RepositoryDb<Classification>
    {
        public ClassificationItemRepository(ISession session)
            : base(session)
        {
        }
    }
}
