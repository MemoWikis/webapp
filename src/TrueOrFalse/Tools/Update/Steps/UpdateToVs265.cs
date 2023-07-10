using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs265
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE message DROP FOREIGN KEY FK_TrainingPlan_id;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"DROP TABLE trainingplan;"
            ).ExecuteUpdate();
    }
}