using System;
using System.Collections.Generic;
using NHibernate;

public class DateRepo : RepositoryDbBase<Date>
{
    public DateRepo(ISession session) : base(session)
    {
    }

    public IList<Date> GetBy(int userId, bool onlyUpcoming = false, bool onlyPrevious = false)
    {
        var queryOver = _session.QueryOver<Date>()
            .Where(d => d.User.Id == userId);

        if (onlyUpcoming)
            queryOver.Where(d => d.DateTime >= DateTime.Now);

        if (onlyPrevious)
            queryOver.Where(d => d.DateTime < DateTime.Now);

        return queryOver.List();
    }
}