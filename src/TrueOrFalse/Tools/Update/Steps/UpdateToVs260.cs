using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs260
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE questioninset DROP FOREIGN KEY FKD34040F20054BCB;"
            ).ExecuteUpdate();

        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE questioninset DROP FOREIGN KEY FKD34040F3A925A27;"
            ).ExecuteUpdate();

        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE questioninset;"
            ).ExecuteUpdate();
    }
}