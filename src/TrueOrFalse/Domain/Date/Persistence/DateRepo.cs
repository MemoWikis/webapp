using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

public class DateRepo : RepositoryDbBase<Date>
{
    public DateRepo(ISession session) : base(session)
    {
    }

    public override void Create(Date date)
    {
        base.Create(date);
        Flush();
        UserActivityAdd.CreatedDate(date);
    }

    public IList<Date> GetBy(int[] userIds, bool onlyUpcoming = false, bool onlyPrevious = false)
    {
        var queryOver = _session.QueryOver<Date>()
            .WhereRestrictionOn(u => u.User.Id).IsIn(userIds);

        if (onlyUpcoming)
            queryOver.Where(d => d.DateTime >= DateTime.Now);

        if (onlyPrevious)
            queryOver.Where(d => d.DateTime < DateTime.Now);

        queryOver = queryOver.OrderBy(q => q.DateTime).Asc;

        return queryOver.List();
    }

    public IList<Date> GetBy(int userId, bool onlyUpcoming = false, bool onlyPrevious = false)
    {
        return GetBy(new [] {userId}, onlyUpcoming, onlyPrevious);
    }

    public IList<Date> GetBySet(int setId)
    {
        return _session.QueryOver<Date>()
            .JoinQueryOver<Set>(d => d.Sets)
            .Where(s => s.Id == setId)
            .List<Date>();
    }

    public int AmountOfPreviousItems(int userId)
    {
        return _session.QueryOver<Date>()
            .Where(d => d.User.Id == userId)
            .And(d => d.DateTime < DateTime.Now)
            .Select(Projections.RowCount())
            .SingleOrDefault<int>();
    }
}