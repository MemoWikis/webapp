using NHibernate;

internal class UpdateToVs264
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE message DROP FOREIGN KEY FK_TrainingDate_id;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE trainingdate DROP FOREIGN KEY TrainingPlan_id_fk2;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"DROP TABLE trainingdate;"
            ).ExecuteUpdate();
    }
}