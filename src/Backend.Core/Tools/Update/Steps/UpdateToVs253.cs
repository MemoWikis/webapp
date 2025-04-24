using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs253
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE learningsession DROP FOREIGN KEY FK3DateToLearn;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE trainingplan DROP FOREIGN KEY Date_id_fk_sdf3;"
            ).ExecuteUpdate();


        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE useractivity DROP FOREIGN KEY Date_id_FK;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE date DROP FOREIGN KEY TrainingPlan_id_fk_12d;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE date DROP FOREIGN KEY User_id_fk;"
            ).ExecuteUpdate();


        nhibernateSession
            .CreateSQLQuery(
                @"DROP TABLE date;"
            ).ExecuteUpdate();
    }
}