using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs249
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE appaccess;"
            ).ExecuteUpdate();
    }
}