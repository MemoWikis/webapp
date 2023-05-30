using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs251
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE categories_to_sets DROP FOREIGN KEY FK937EA8C2253EAD1B;"
            ).ExecuteUpdate();

        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE categories_to_sets DROP FOREIGN KEY FK937EA8C286903877;"
            ).ExecuteUpdate();

        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE categories_to_sets;"
            ).ExecuteUpdate();
    }
}