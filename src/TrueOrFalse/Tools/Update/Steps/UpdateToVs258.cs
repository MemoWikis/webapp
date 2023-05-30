using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs258
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE questionfeature;"
            ).ExecuteUpdate();
    }
}