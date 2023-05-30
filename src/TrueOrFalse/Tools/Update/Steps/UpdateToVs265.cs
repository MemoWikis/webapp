using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs265
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE message DROP FOREIGN KEY FK_TrainingPlan_id;"
            ).ExecuteUpdate();

        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE trainingplan;"
            ).ExecuteUpdate();
    }
}