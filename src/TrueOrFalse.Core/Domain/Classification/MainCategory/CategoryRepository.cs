using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Seedworks.Lib.Persistance;

namespace TrueOrFalse.Core
{
    public class CategoryRepository : RepositoryDb<Category>
    {
        public CategoryRepository(ISession session) : base(session)
        {
        }
    }
}
