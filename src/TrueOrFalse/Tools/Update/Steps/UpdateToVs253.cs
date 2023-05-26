using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs253
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE learningsession DROP FOREIGN KEY FK3DateToLearn;"
            ).ExecuteUpdate();

        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE learningsession DROP COLUMN DateToLearn_id;"
            ).ExecuteUpdate();

        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE trainingplan DROP FOREIGN KEY Date_id_fk_sdf3;"
            ).ExecuteUpdate();


        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE useractivity DROP FOREIGN KEY Date_id_FK;"
            ).ExecuteUpdate();


        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE date;"
            ).ExecuteUpdate();
    }
}