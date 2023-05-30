using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs246
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE membership;"
            ).ExecuteUpdate();
    }
}