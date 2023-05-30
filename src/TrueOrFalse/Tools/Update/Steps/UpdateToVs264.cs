using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs264
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE message DROP FOREIGN KEY FK_TrainingDate_id;"
            ).ExecuteUpdate();

        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"ALTER TABLE trainingdate DROP FOREIGN KEY TrainingPlan_id_fk2;"
            ).ExecuteUpdate();

        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE trainingdate;"
            ).ExecuteUpdate();
    }
}