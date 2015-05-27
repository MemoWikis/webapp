using System;
using NHibernate;

public class GetTotalCategories : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public GetTotalCategories(ISession session){
        _session = session;
    }

    public int Run(){
        return (int)_session.CreateQuery("SELECT Count(Id) FROM Category").UniqueResult<Int64>();
    }
}