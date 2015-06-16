using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;

public class DateRepo : RepositoryDbBase<Date>
{
    public DateRepo(ISession session) : base(session)
    {
    }

    public IList<Date> GetBy(int[] userIds, bool onlyUpcoming = false, bool onlyPrevious = false)
    {
        var queryOver = _session.QueryOver<Date>()
            .WhereRestrictionOn(u => u.User.Id).IsIn(userIds);

        if (onlyUpcoming)
            queryOver.Where(d => d.DateTime >= DateTime.Now);

        if (onlyPrevious)
            queryOver.Where(d => d.DateTime < DateTime.Now);

        queryOver = queryOver.OrderBy(q => q.DateTime).Desc;

        return queryOver.List();
    }

    public IList<Date> GetBy(int userId, bool onlyUpcoming = false, bool onlyPrevious = false)
    {
        return GetBy(new [] {userId}, onlyUpcoming, onlyPrevious);
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