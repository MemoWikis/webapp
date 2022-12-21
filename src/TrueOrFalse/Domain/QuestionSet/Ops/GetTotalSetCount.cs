using System;
using NHibernate;
using NHibernate.Criterion;

public class GetTotalSetCount : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public GetTotalSetCount(ISession session){
        _session = session;
    }

    public int Run(){
        return (int)_session.CreateQuery("SELECT Count(Id) FROM Set").UniqueResult<Int64>();
    }

    public int Run(int creatorId)
    {
        return _session.QueryOver<Set>()
            .Where(s => s.Creator != null && s.Creator.Id == creatorId)
            .Select(Projections.RowCount())
            .FutureValue<int>()
            .Value;
    }
}