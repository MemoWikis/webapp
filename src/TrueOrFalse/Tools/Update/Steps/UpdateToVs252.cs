using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs252
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE date_to_sets;"
            ).ExecuteUpdate();
    }
}