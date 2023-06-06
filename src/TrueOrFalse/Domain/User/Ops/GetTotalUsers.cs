using NHibernate;

public class GetTotalUsers : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public GetTotalUsers(ISession session){
        _session = session;
    }

    public int Run(){
        return (int)_session.CreateQuery("SELECT Count(Id) FROM User").UniqueResult<Int64>();
    }
}