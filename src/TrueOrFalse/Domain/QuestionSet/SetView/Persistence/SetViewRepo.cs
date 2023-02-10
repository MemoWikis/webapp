
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using Seedworks.Lib.Persistence;

public class SetViewRepo : RepositoryDb<SetView>
{
    public SetViewRepo(ISession session) : base(session) { }
}
