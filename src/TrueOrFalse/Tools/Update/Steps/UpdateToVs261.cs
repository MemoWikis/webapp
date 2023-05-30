using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs261
{
    public static void Run()
    {
        //Sl.Resolve<ISession>()
        //    .CreateSQLQuery(
        //        @"ALTER TABLE learningsession DROP FOREIGN KEY FK946A748A5C428B0;"
        //    ).ExecuteUpdate();

        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE useractivity DROP FOREIGN KEY Set_id_FK;"
            ).ExecuteUpdate();

        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE questionset;"
            ).ExecuteUpdate();
    }
}