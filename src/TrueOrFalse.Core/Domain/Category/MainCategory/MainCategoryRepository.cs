using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Seedworks.Lib.Persistance;

namespace TrueOrFalse.Core
{
    public class MainCategoryRepository : RepositoryDb<MainCategory>
    {
        public MainCategoryRepository(ISession session) : base(session)
        {
        }
    }
}
