using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs263
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE setview;"
            ).ExecuteUpdate();
    }
}