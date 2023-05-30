using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs257

{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE useractivity DROP FOREIGN KEY Game_id_FK;"
            ).ExecuteUpdate();

        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE game;"
            ).ExecuteUpdate();
    }
}