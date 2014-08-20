using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;

namespace TrueOrFalse
{
    public class ReferenceRepository : RepositoryDb<Reference>
    {
        public ReferenceRepository(ISession session) : base(session)
        {
        }

    }
}