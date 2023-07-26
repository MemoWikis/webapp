using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs266
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE questionview DROP FOREIGN KEY FK_WidgetView;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"DROP TABLE widgetview;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE useractivity DROP COLUMN Date_id, DROP COLUMN Game_id, DROP COLUMN Set_id;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE questionview DROP COLUMN Round_id, DROP COLUMN WidgetView_id, DROP COLUMN Player_id;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE message DROP COLUMN TrainingDate_id, DROP COLUMN TrainingPlan_id;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE learningsession DROP COLUMN DateToLearn_id, DROP COLUMN SetToLearn_id;"
            ).ExecuteUpdate();
    }
}