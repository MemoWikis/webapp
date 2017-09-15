
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using Seedworks.Lib.Persistence;

public class SetViewRepo : RepositoryDb<SetView>
{
    public SetViewRepo(ISession session) : base(session) { }

    public int GetViewCount(int setId)
    {
        return _session.QueryOver<SetView>()
            .Select(Projections.RowCount())
            .Where(x => x.Set.Id == setId)
            .FutureValue<int>()
            .Value;
    }

    public IList<AmountPerDay> GetViewsPerDay(int setId, bool excludeAdmins = false)
    {
        //var queryOver = _session.QueryOver<SetView>()
        //    .Where(x => x.Set.Id == setId);

        //if (excludeAdmins)
        //    queryOver = queryOver.And(x => !x.User.IsInstallationAdmin);

        return _session.QueryOver<SetView>()
            .Where(x => x.Set.Id == setId)
            .List()
            .GroupBy(x => x.DateCreated.Date)
            .Select(d => new AmountPerDay
            {
                DateTime = d.Key,
                Value = d.Count()
            })
            .ToList();
    }
}
