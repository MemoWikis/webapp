using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs252
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE date_to_sets DROP FOREIGN KEY FKCB583B0E93032C55;"
            ).ExecuteUpdate();

        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE date_to_sets DROP FOREIGN KEY FKCB583B0EBC787FC9;"
            ).ExecuteUpdate();

        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE date_to_sets;"
            ).ExecuteUpdate();
    }
}