using NHibernate;
using Seedworks.Lib.Persistence;

public class WidgetViewRepo : RepositoryDb<WidgetView>
{
    public WidgetViewRepo(ISession session) : base(session)
    {
    }
}