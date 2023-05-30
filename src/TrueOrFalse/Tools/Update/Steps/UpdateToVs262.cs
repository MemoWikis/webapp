using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs262
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE setvaluation;"
            ).ExecuteUpdate();
    }
}